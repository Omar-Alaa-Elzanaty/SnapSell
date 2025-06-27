# Use the ASP.NET Core runtime image (includes everything needed for web apps)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set working directory
WORKDIR /app

# Copy published files into the container
COPY ./ ./

# Expose the port your app listens on (e.g., 5080)
EXPOSE 5080

# Run the application
ENTRYPOINT ["dotnet", "SnapSell.API.dll", "--urls", "http://0.0.0.0:5080"]