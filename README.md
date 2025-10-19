Простой web-сервис для управления сотрудниками компании.  
Реализовано на .NET 8, используется Dapper и PostgreSQL.

Для работы проекта нужно:
1. Открыть PostgreSQL.

2. Создать пустую БД:
CREATE DATABASE testdb;

3. В appsettings.json задать строку подключения:
"ConnectionStrings": {
  "TestDb": "Host=localhost;Port=5432;Database=testdb;Username=postgres;Password=1234"
}

4. Выполнить SQL-скрипт init_db.sql, чтобы создать структуру
Через терминал:
psql -U postgres -d testdb -f init_db.sql

Либо через pgadmin просто запустить скрипт

5. Запуск API