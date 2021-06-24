# Slot ApiGateway
​
Application that is Api Gateway for slot api. http://test.draliacloud.net/api/availability/
​
## Prerequisites
​
- `dotnet 5.0`
  ​
  The minimum dotnet sdk version required for this project is 5.0. There are many tutorials on the internet that explains how to install it according to the OS you are using. Follow this guide https://dotnet.microsoft.com/download/dotnet/5.0
  
## Run application locally
​
We will describe some ways you can run `slot-gateway` locally in your machine
​
### Run using IDE
​
- In the IDE configuration, set the following values in `ASPNETCORE_ENVIRONMENT=Development`

### Run using command line

- Run the command below to in the command line 
  ```
  dotnet run -e ASPNETCORE_ENVIRONMENT=Development --project src/Slot.Gateway/Slot.Gateway.csproj
  ```
  > **Note1:** App url is "http://localhot:5000"
  > 
  > **Note2:** To stop press `Ctrl+C`


### Run as Docker container
​
- In order to build the docker image run
  ```
  docker build -t slot-gateway .
  ```
​
- To start the docker container run
  ```
  docker run -d --rm --name slot-gateway-project \
   -p 5000:80 \
   -e ASPNETCORE_ENVIRONMENT=Development \
   slot-gateway:latest
  ```
  > To stop run
  > ```
  > docker stop slot-gateway-project
  > ```
​
## Test using Swagger or cURL
​
- Using `Swagger UI` http://localhost:5000/swagger/index.html

- Using `curl`
  ```
  curl -X GET "https://localhost:5001/api/v1/Slot/weekly/20210614" -H  "accept: text/plain"

  ```
