#Get base SDK image from microsoft
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the CSPROJ file and restore any dependencies (via NUGET)
COPY *.csproj ./
RUN dotnet restore

# Copy the project files and build our release
COPY . ./
RUN dotnet publish -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR
/app
EXPOSE 80
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Solanum-back.dll"]