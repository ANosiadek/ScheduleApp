Dokumentacja Programu - Harmonogram Wydarzeń
Opis projektu
Harmonogram Wydarzeń to aplikacja webowa służąca do zarządzania spotkaniami i użytkownikami. Aplikacja umożliwia rejestrację użytkowników, planowanie spotkań oraz zarządzanie nimi zarówno przez zwykłych użytkowników, jak i administratorów.

Technologie i konfiguracja
Aplikacja została napisana w języku C# z użyciem ASP.NET Core. Zapis danych odbywa się do pliku tekstowego (aplikacja korzysta z lokalnych plików tekstowych do przechowywania danych-eliminuje to zależność od zewnętrznych baz danych). Aplikacja jest skonfigurowana do uruchamiania w środowisku deweloperskim. Konfiguracja serwera IIS Express oraz adresy URL do uruchamiania aplikacji znajdują się w pliku launchSettings.json w folderze Properties.

Struktura projektu
Projekt składa się z następujących elementów:

Controllers: Zawiera logikę obsługi żądań HTTP.
Services: Zawiera logikę do zarządzania danymi.
Views: Zawiera widoki używane do renderowania interfejsu użytkownika.
wwwroot: Zawiera statyczne zasoby takie jak pliki CSS, JS, itp.
Properties: Zawiera ustawienia konfiguracji aplikacji.
HomeController: Obsługuje logowanie użytkowników oraz wyświetlanie stron informacyjnych.

Index(): Wyświetla formularz logowania.
Index(string username, string password): Obsługuje logowanie użytkownika.
Error(): Wyświetla stronę błędu.
Creators(): Wyświetla stronę twórców aplikacji.
AccountController
Obsługuje rejestrację użytkowników oraz wyświetlanie harmonogramu spotkań.

Register(): Wyświetla formularz rejestracji.
Register(string username, string password): Obsługuje rejestrację użytkownika.
Schedule(): Przekierowuje do harmonogramu spotkań.
Logout(): Wylogowuje użytkownika.
AdminController
Obsługuje logowanie administratorów oraz zarządzanie użytkownikami i wydarzeniami.

Login_admin(): Wyświetla formularz logowania administratora.
Login_admin(string password): Obsługuje logowanie administratora.
Panel_admin(): Wyświetla panel administracyjny.
DeleteUser(string userId): Usuwa użytkownika.
EditEvent(int id): Wyświetla formularz edycji wydarzenia.
EditEvent(WorkEvent workEvent): Aktualizuje wydarzenie.
DeleteEvent(int eventId): Usuwa wydarzenie.
ChangePassword(string userId): Wyświetla formularz zmiany hasła.
ChangePassword(ChangePasswordViewModel model): Zmienia hasło użytkownika.
Logout(): Wylogowuje administratora.
WorkEventsController
Obsługuje zarządzanie wydarzeniami użytkowników.
App.UseSession() : włącza obsługę sesji w potoku HTTP (możliwość przechowywania i zarządzania danymi sesyjnymi-identyfikatory użytkowników)
Index(): Wyświetla wydarzenia zalogowanego użytkownika.
Create(): Wyświetla formularz tworzenia wydarzenia.
Create(string title, DateTime start, DateTime end): Tworzy nowe wydarzenie.
Edit(int id): Wyświetla formularz edycji wydarzenia.
Edit(int id, string title, DateTime start, DateTime end): Edytuje wydarzenie.
Delete(int id): Usuwa wydarzenie.

Services
FileDataService
Obsługuje operacje na danych użytkowników i wydarzeń zapisanych w plikach tekstowych.

GetUsersAsync(): Odczytuje użytkowników z pliku.
GetEventsAsync(): Odczytuje wydarzenia z pliku.
SaveUsersAsync(List<ApplicationUser> users): Zapisuje użytkowników do pliku.
SaveEventsAsync(List<WorkEvent> events): Zapisuje wydarzenia do pliku.
DeleteEventAsync(int id): Usuwa wydarzenie.
DeleteUserAsync(string id): Usuwa użytkownika.
Widoki
Widoki są zapisane w folderze Views i obsługują wyświetlanie interfejsu użytkownika. Przykładowe widoki to formularze logowania, rejestracji, tworzenia i edycji wydarzeń oraz panel administracyjny.
Async/await(): metoda asynchroniczna użyta do odczytu i zapisu danych z plików tekstowych. Pozwala na lepszą obsługę I/O i zapewnia, że operacja nie blokuje wątku głównego aplikacji.
