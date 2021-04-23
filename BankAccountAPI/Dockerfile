

# Stage 1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /build
COPY . .
RUN dotnet publish -c Release -o /app
COPY PythonScript/Script.py /app

# Stage 2
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
COPY --from=build /app .
EXPOSE 80

# Python dependencies
RUN apt-get update
RUN apt-get install python3 --assume-yes
RUN apt-get install python3-pip --assume-yes
RUN pip3 install python-dateutil
RUN pip3 install fints

# environment variables
ENV PYTHON_PATH=/usr/bin/python3
ENV PYTHON_SCRIPT_PATH=/app/Script.py

ENTRYPOINT ["dotnet", "BankAccountAPI.dll"]