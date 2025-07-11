using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using MongoDB.Bson;
using MongoDB.Driver;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Presistance.Context;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SnapSell.API
{
    public static class DependancyInjection
    {
        public static IServiceCollection DepedencyInjectionService(this IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerGen(c =>
            {
                c.SchemaGeneratorOptions = new SchemaGeneratorOptions
                {
                    UseInlineDefinitionsForEnums = true,
                };

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SnapSell API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecureKey"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services
                .AddHttpContextAccessor()
                .AddMemoryCache()
                .AddCors(config)
                .AddIIS();

            return services;
        }

        private static IServiceCollection AddIIS(this IServiceCollection services)
        {
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;
            });
            return services;
        }
        private static IServiceCollection AddCors(this IServiceCollection services, IConfiguration config)
        {
            var originCors = config.GetSection("APP:CorsOrigins").Get<string>()?.Split(';') ?? [];

            services.AddCors(options =>
            {
                options.AddPolicy("DEVELOPMENT", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });

                options.AddPolicy("PRODUCTION", builder =>
                {
                    builder.WithOrigins(originCors)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin();
                });
            });


            return services;
        }
        public static async Task EnsureProductTextIndexCreatedAsync(this IServiceProvider services)
        {
            try
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                var mongoClient = services.GetRequiredService<IMongoClient>();
                var settings = services.GetRequiredService<IMongoDbSettings>();
                var database = mongoClient.GetDatabase(settings.DatabaseName);
                var productCollection = database.GetCollection<Product>("Products");
                
                var existingIndexes = await productCollection.Indexes.ListAsync();
                var indexes = await existingIndexes.ToListAsync();
                
                var hasTextIndex = indexes.Any(index =>
                    index.Contains("weights") &&
                    index["weights"].AsBsonDocument.Names.Contains("EnglishName"));
                
                if (!hasTextIndex)
                {
                    var indexKeys = Builders<Product>.IndexKeys
                        .Text(x => x.EnglishName)
                        .Text(x => x.ArabicName)
                        .Text(x => x.ArabicDescription)
                        .Text(x => x.EnglishDescription)
                        .Text("Brand.Name")
                        .Text("Category.Name");
                
                    var weights = new BsonDocument
                    {
                        { "EnglishName", 10 },
                        { "ArabicName", 8 },
                        { "EnglishDescription", 5 },
                        { "ArabicDescription", 5 },
                        { "Brand.Name", 3 },
                        { "Category.Name", 2 }
                    };
                
                    var indexModel = new CreateIndexModel<Product>(
                        keys: indexKeys,
                        options: new CreateIndexOptions
                        {
                            Weights = weights
                        });
                
                    await productCollection.Indexes.CreateOneAsync(indexModel);
                    logger.LogInformation("MongoDB product text index created successfully.");
                }
                else
                {
                    logger.LogInformation("MongoDB product text index already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("MongoDB index creation failed. See inner exception for details.", ex);
            }
        }
    }
}
