# Група ПД123 Мережеве програмування Core 6.0

Розробка чату і різних приколів 

## Запуск проекта

Виконайте наступні кроки

1. Установка Android Studio Dolphin | 2021.3.1 або вище - https://developer.android.com/studio

2. Установити dotnet ef .NET CLI 

```
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
dotnet ef --info 
``` 

3. Ставимо пакети і додаємо міграцію. Перейти в папку `Web.Pizza/Web.Pizza`:

```
dotnet ef migrations add "Init Database" -c AppEFContext -p ../Data.Pizza/Data.Pizza.csproj -s Web.Pizza.csproj

dotnet ef database update -c AppEFContext -p ../Data.Pizza/Data.Pizza.csproj -s Web.Pizza.csproj

```

4. Перейти в папку `Web.Pizza/Web.Pizza`. Установка ASP.NET core 6.0 або вище - https://dot.net

```
dotnet restore
dotnet build
dotnet watch run
```

5. Вікриваємо http://localhost:5000 у веб браузері.



