using grpc.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<ChatService>();
app.MapGet("/", () => "gRPC Server is running. Use a gRPC client to connect.");

app.Run();
