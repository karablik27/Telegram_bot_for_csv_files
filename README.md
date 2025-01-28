# Telegram Bot for Geraldic Signs

This project is a Telegram bot written in C# that processes and manages information about heraldic (geraldic) signs. The bot is built using the Telegram.Bot library and works in polling mode. It supports user interaction through commands and requests, manages file uploads and downloads, and works with a structured dataset of heraldic signs. Additionally, it provides options for sorting, filtering, and retrieving data from a CSV file.

### Functionality
The bot is designed to work with a dataset of heraldic symbols stored in a CSV file. The dataset contains information such as the name of the symbol, its type (e.g., flag or coat of arms), an associated image identifier, description, semantic meaning, certificate holder name, registration date, and unique global ID. The bot allows users to:
- Search for heraldic signs by name.
- Filter symbols based on type (e.g., only flags or coats of arms).
- Retrieve detailed descriptions and semantic meanings of symbols.
- Sort entries based on registration date or other parameters.
- Download or view structured information about heraldic signs.

### Technologies Used
The bot is built using:
- **C# and .NET 6.0** for backend logic.
- **Telegram.Bot** library for interaction with the Telegram API.
- **CSV file processing** for structured data storage.
- **LINQ** for filtering and sorting data dynamically.
- **JSON configuration** for storing API keys and bot settings.

### Installation and Setup
#### Prerequisites
- .NET 6.0 or higher must be installed.
- A Telegram bot token (obtained from [BotFather](https://t.me/botfather)).

#### Steps to Run
1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/tg_bot_csv.git
   cd tg_bot_csv
   ```
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. Configure the bot token in `appsettings.json`:
   ```json
   {
       "BotToken": "YOUR_TELEGRAM_BOT_TOKEN"
   }
   ```
4. Ensure the `geraldic-signs.csv` file is present in the project directory.
5. Run the bot:
   ```sh
   dotnet run
   ```

### Project Structure
```
📂 Karabelnikov_LastKDZ_Var8
 ├── 📂 TG_Bot_Help        # Command processing
 ├── 📂 TG_Bot_Main        # Core bot logic
 ├── 📂 TG_Bot_Methods     # Data processing functions
 ├── 📜 Telegram_Bot.cs    # Main bot logic
 ├── 📜 Program.cs         # Entry point
 ├── 📜 geraldic-signs.csv # Heraldic signs dataset
```

### Commands
- `/start` - Start the bot
- `/help` - Show available commands
- `/search <name>` - Find a heraldic sign by name
- `/list` - Show all available signs
- `/filter <type>` - Filter by type (e.g., coat of arms, flag)
- `/sort <param>` - Sort data by registration date or another parameter



