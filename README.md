Многопоточный сервер для обмена текстовыми сообщениями.

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
