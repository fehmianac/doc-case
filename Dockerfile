FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build-env
ENV PROJECT_NAME="Slot.Gateway"
WORKDIR /app
COPY . ./

#RUN dotnet test
RUN dotnet restore src/$PROJECT_NAME/$PROJECT_NAME.csproj \
  && dotnet publish src/$PROJECT_NAME/$PROJECT_NAME.csproj -c Release -o /app/out \
  && touch /app/scm_metadata.json
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim

#Setup timezone to Europe/Madrid
RUN apt-get update \
  && apt-get install -y --no-install-recommends tzdata=2021a-0+deb10u1 curl=7.64.0-4+deb10u2\
  && apt-get clean \
  && rm -rf /var/lib/apt/lists/*

#ENV TZ Europe/Madrid

WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /app/scm_metadata.json .
EXPOSE 80
ENTRYPOINT ["dotnet", "Slot.Gateway.dll"]
