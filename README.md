**Multithreaded server for text messages.**

*A test task for a company developing dispatch software.*

The server is implemented as a Windows Forms application with the following functionality:

- Recording errors and state transitions (start/stop) in the Windows log
- After server starts the following are set: server port and name
- Storage of connected clients (if the client is inactive for a long time, it is automatically excluded from the list of clients registered on the server)

The client is implemented as a Windows Forms application with the following functionality:

- Client starts the following are set: server IP address and port
- Display list of clients registered on the server
- Sending messages to the selected client

The protocol for exchanging with the server implements the following commands:

- Registering on the server with the specified name
- Disconnecting the client with the specified name from the server
- Sending a message to the client

Transport protocol for exchanging - UDP, implementation language - C#, .NET version - 2.0.



**Многопоточный сервер для обмена текстовыми сообщениями.**

*Тестовое задание в компанию занимающейся разработкой диспетчерского программного обеспечения.* 

Сервер реализован как Windows Forms приложение со следующим функционалом:

- Запись об ошибках и переходах между состояниями (старт/стоп) в журнал windows
- После запуска сервера задается: порт и имя сервера
- Учет подключенных клиентов (при длительной неактивности клиент автоматически исключается из списка зарегистрированных на сервере)

Клиент реализован как Windows Forms приложение со следующим функционалом:

- После запуска клиента задается: IP адрес и порт сервера
- Отображение списка зарегистрированных на сервере клиентов
- Отправка выбранному клиенту сообщений

Протокол обмена с сервером реализует следующие команды:

- Регистрация на сервере с заданным именем
- Отключение клиента с заданным именем от сервера
- Отправка сообщения клиенту

Транспортный протокол обмена - UDP, язык реализации - C#, версия .NET - 2.0.


