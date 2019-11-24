# MultiYear.MediatR.Example
Sample showing how you could go about using MediatR to change which command handler is used during runtime

## Running the application locally.
1) Set WebApi as startup
2) Navigate to [localhost:####]/swagger
3) use Swagger UI to run the api/test/mediatR endpoint

Currently MediatR fails to resolve the IRequestHandler when the IRequest was created by reflection.
