.PHONY: build run migrate migration clean restore db-up db-down

# Makefile for SampleApi (.NET 8 + PostgreSQL + EF Core)

# Variables
PROJECT = SampleApi
MIGRATION_NAME ?= NewMigration
CONNECTION = "Host=localhost;Port=5432;Database=sample;Username=admin;Password=supersecret"

# Default command
.DEFAULT_GOAL := help

help:
	@echo ""
	@echo "Available commands:"
	@echo "  make restore          Restore NuGet packages"
	@echo "  make build            Build the project"
	@echo "  make run              Run the application"
	@echo "  make migrate          Apply latest migrations"
	@echo "  make migration name=Init   Create new EF migration"
	@echo "  make clean            Clean build artifacts"
	@echo "  make db-up            Start local PostgreSQL (Docker)"
	@echo "  make db-down          Stop local PostgreSQL container"
	@echo ""

# Commands
restore:
	dotnet restore

build:
	dotnet build

run:
	dotnet run --project $(PROJECT)

migrate:
	dotnet ef database update --project $(PROJECT)

migration:
	dotnet ef migrations add $(MIGRATION_NAME) --project $(PROJECT)

clean:
	dotnet clean

# --- PostgreSQL via Docker ---
db-up:
	docker run --name sample-postgres -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=supersecret -e POSTGRES_DB=sample -p 5432:5432 -d postgres:16-alpine

db-down:
	docker rm -f sample-postgres
