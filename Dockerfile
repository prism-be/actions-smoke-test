# Set the base image as the .NET 6.0 SDK (this includes the runtime)
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env

# Copy everything and publish the release (publish implicitly restores and builds)
WORKDIR /app
COPY . ./
RUN dotnet publish ./Prism.GithubActions.SmokeTest/Prism.GithubActions.SmokeTest.csproj -c Release -o out --no-self-contained

# Label the container
LABEL maintainer="Simon Baudart <simon@contact-me.be>"
LABEL repository="https://github.com/prism-be/actions-smoke-test"
LABEL homepage="https://github.com/prism-be/"

# Label as GitHub action
LABEL com.github.actions.name="Simple Smoke Test"
# Limit to 160 characters
LABEL com.github.actions.description="Run a simple smoke test on a web application based on sitemap."
# See branding:
# https://docs.github.com/actions/creating-actions/metadata-syntax-for-github-actions#branding
LABEL com.github.actions.icon="activity"
LABEL com.github.actions.color="orange"

# Relayer the .NET SDK, anew with the build output
FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "/Prism.GithubActions.SmokeTest.dll" ]