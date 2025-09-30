# AI Enhanced Journal

Welcome to AI Enhanced Journal, a personal journaling application that leverages AI to provide insights, prompts, and suggestions. This project is built using C# and .NET Core, with a SQLite database for storing journal entries.

## Table of Contents

- [Getting Started](#getting-started)
- [Features](#features)
- [Usage](#usage)
  - [Creating a New Journal Entry](#creating-a-new-journal-entry)
  - [Viewing All Entries](#viewing-all-entries)
  - [Searching Entries](#searching-entries)
  - [Getting AI Insights for an Entry](#getting-ai-insights-for-an-entry)
  - [Generating Journal Prompts](#generating-journal-prompts)
- [AI Integration](#ai-integration)
  - [OpenAI API](#openai-api)
- [Contributing](#contributing)

## Getting Started

1. Clone or download the repository
2. Navigate to the project directory
3. Restore the project dependencies
4. Build the project
5. Run the application

## Features

- Create, view, and search journal entries
- Generate AI insights based on journal content
- Generate journal prompts based on mood and topic
- Integrated with OpenAI API for AI-powered features

## Usage

### Creating a New Journal Entry

1. In the main menu, choose option 1: "Create new journal entry"
2. Enter the title for your journal entry
3. Enter your mood (optional)
4. Enter your journal content (press Enter twice to finish)
5. The application will generate AI insights based on your journal content and display them

### Viewing All Entries

1. In the main menu, choose option 2: "View all entries"
2. The application will display a list of all journal entries, including their titles, dates, and moods

### Searching Entries

1. In the main menu, choose option 3: "Search entries"
2. Enter a keyword or phrase to search for in the journal entries
3. The application will display a list of matching journal entries, including their titles, dates, and moods

### Getting AI Insights for an Entry

1. In the main menu, choose option 4: "Get AI insights for an entry"
2. Enter the ID of the journal entry you want to get insights for
3. The application will display the journal entry content and generate new AI insights based on the content

### Generating Journal Prompts

1. In the main menu, choose option 5: "Generate journal prompts"
2. Enter your current mood (optional)
3. Enter a topic you'd like to explore (optional)
4. The application will generate a list of journal prompts based on your input

## AI Integration

AI Enhanced Journal integrates with the OpenAI API for AI-powered features.

### OpenAI API

To use the OpenAI API, you need to sign up for an API key at [https://platform.openai.com/account/api-keys](https://platform.openai.com/account/api-keys).

Once you have your API key, create a file named `appsettings.json` in the root directory of the project and add the following configuration:

```json
{
  "OpenAI": {
    "ApiKey": "your-openai-api-key-here"
  }
}
```

Replace `your-openai-api-key-here` with your actual OpenAI API key.

## Contributing

Contributions are welcome! If you have any ideas or improvements, feel free to open an issue or submit a pull request.

Please note that this project is a personal project and may not be actively maintained. If you encounter any bugs or issues, please let me know and I'll do my best to help.

Thank you for using AI Enhanced Journal!
