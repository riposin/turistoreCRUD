# Turistore CRUD
WPF, XAML, and SQLite

# Run Vweb for SQLite
1. Clone the repo.
2. Execute this PowerShell script ~/TuriStore/Data/script_new_sqlite3_db.ps1
3. Follow the few steps indicated in by the script.
4. Visual Studio is usually configured to restore NuGet packages; if that is not your case, restore the packages manually.
5. Clean solution.
6. Build solution.
7. Run.

# For new entities
If your controller will serve views and use the Visits layout, be sure to add code to make Vweb settings and Vweb locales available to the views of the controller. See the PreregistrationController.cs for more detail.

# For changes in DB
Make sure you write the changes to SQL script.