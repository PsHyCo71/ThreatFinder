# Contributing to ThreatFinder

Thank you for your interest in contributing to ThreatFinder!

ThreatFinder is an open-source, cross-platform desktop application for checking files and URLs against threat intelligence databases. Contributions are welcome, whether they are bug fixes, new features, improvements to the UI, or additional threat intelligence providers.

## Table of Contents

- [Contributing to ThreatFinder](#contributing-to-threatfinder)
  - [Table of Contents](#table-of-contents)
  - [Getting Started](#getting-started)
  - [Project Structure](#project-structure)
  - [Making Changes](#making-changes)
  - [Adding a Threat Intelligence Provider](#adding-a-threat-intelligence-provider)
  - [API Keys \& Sensitive Data](#api-keys--sensitive-data)
  - [Commit Guidelines](#commit-guidelines)
  - [Pull Requests](#pull-requests)
  - [Bug Reports](#bug-reports)
  - [Feature Requests](#feature-requests)
  - [Code Style](#code-style)
  - [License](#license)

## Getting Started

Before contributing, make sure you have the required development environment:

* Windows 10/11 (x64) or Linux (x64)
* .NET SDK 10.0 or later
* Git

Clone the repository:

```bash
git clone https://github.com/PsHyCo71/ThreatFinder.git
cd ThreatFinder
```

Run the project with:

```bash
dotnet run
```

Make sure the project builds and runs correctly before making changes.

## Project Structure

ThreatFinder follows the MVVM pattern and separates the application into different responsibilities.

The main parts of the project are:

* `ThreatFinder.Core` — contains the core scanning logic and application-independent functionality.
* `ThreatFinder.Providers` — contains implementations of threat intelligence providers.
* `ThreatFinder.UI` — contains the Avalonia UI and ViewModels.

When possible, changes should be made in the appropriate project rather than putting application logic directly into the UI.

## Making Changes

Before starting significant work, consider opening an issue to discuss the change.

For smaller changes, you can create a branch directly from the latest version of `main`:

```bash
git checkout main
git pull
git checkout -b feature/your-feature
```

For bug fixes, use a descriptive branch name such as:

```bash
git checkout -b fix/scan-error
```

Try to keep each branch focused on a single feature, bug fix, or improvement.

## Adding a Threat Intelligence Provider

ThreatFinder uses the `IThreatIntelProvider` abstraction to make it possible to add additional threat intelligence sources.

If you want to add a new provider:

1. Create the provider in `ThreatFinder.Providers`.
2. Implement `IThreatIntelProvider`.
3. Keep provider-specific API logic inside the provider.
4. Do not place API-specific code inside the UI.
5. Handle API errors and unexpected responses appropriately.
6. Make sure the provider does not expose or store API keys in source code.
7. Update the relevant documentation if the new provider changes how ThreatFinder is used.

New providers should respect the terms and usage policies of the service they interact with.

## API Keys & Sensitive Data

**Never commit API keys, credentials, or other sensitive information to the repository.**

ThreatFinder stores the user's MalwareBazaar and URLhaus Auth-Keys locally in `appsettings.json`. This file is excluded from version control.

Before committing your changes, make sure that you have not accidentally included:

* MalwareBazaar Auth-Keys
* URLhaus Auth-Keys
* `appsettings.json`
* Personal credentials
* Tokens or passwords
* Other private configuration data

If you accidentally expose a secret, revoke it immediately and generate a new one.

## Commit Guidelines

Use clear and descriptive commit messages.

Good:

```text
Add URLhaus provider error handling
```

```text
Fix scan result display on Linux
```

```text
Add loading state to scan button
```

Avoid vague messages such as:

```text
fixed stuff
```

```text
updates
```

```text
some changes
```

Try to keep commits focused and avoid combining unrelated changes into a single commit.

## Pull Requests

Before opening a Pull Request:

1. Make sure the project builds successfully.
2. Test the changes you made.
3. Make sure no sensitive information is included.
4. Keep the Pull Request focused on one change.
5. Update the documentation when necessary.
6. Write a clear description of what was changed and why.

Push your branch:

```bash
git push origin feature/your-feature
```

Then open a Pull Request against the `main` branch.

A good Pull Request description should briefly explain:

* What was changed
* Why it was changed
* How it was tested
* Any limitations or known issues

## Bug Reports

If you find a bug, please open an issue using the **Bug Report** template.

Include as much relevant information as possible, such as:

* Operating system
* ThreatFinder version
* Steps to reproduce the problem
* Expected behavior
* Actual behavior
* Error messages or logs
* Screenshots, when useful

Do **not** include API keys, passwords, private URLs, or other sensitive information in an issue.

## Feature Requests

Feature ideas are welcome.

Before requesting a feature, check whether a similar issue already exists.

When opening a feature request, explain:

* What the feature should do
* Why it would be useful
* How you expect it to work
* Any relevant examples or use cases

For larger changes, it is recommended to discuss the idea in an issue before implementing it.

## Code Style

Keep the existing coding style of the project.

In particular:

* Use clear and descriptive names.
* Keep methods and classes focused on a single responsibility.
* Follow the existing MVVM architecture.
* Keep UI code separate from scanning and provider logic.
* Prefer asynchronous APIs for network operations.
* Handle expected exceptions appropriately.
* Avoid unnecessary dependencies.
* Do not hard-code API keys or other secrets.

When modifying existing code, prefer consistency with the surrounding code over introducing a completely different style.

## License

By contributing to ThreatFinder, you agree that your contributions will be distributed under the same license as the project.

ThreatFinder is distributed under the **PolyForm NonCommercial License 1.0.0**.

Please read the [`LICENSE.md`](https://github.com/PsHyCo71/ThreatFinder/blob/main/LICENSE.md) file before contributing.

Thank you for contributing to ThreatFinder! 🔍
