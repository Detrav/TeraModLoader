# TeraModLoader

Загрузчик модов для игры Tera Online.

Изначально проект задумывался как простой ДПС метр с несколькими настройками, однако в процессе написания понадобился сниффер, а теперь я разрабатываю полноценный движок аддонов для игры тера.

Фактически это оболочка для программ обработки пакетов Tera. Моя Программа занимается перехватом и расшифровкой пакетов, позволяет изолированно запустить ряд модов(программ) и передать им расшифрованные пакеты.

Для поддержания такого проекта мне нужна будет помощь, пишите [**сюда**](https://github.com/Detrav/TeraModLoader/issues).

Позже будут разработаны пара плагинов: Дпс метр, простой с несколькими настройкам и может... что то ещё.
## Установка
1.	Для начала необходимо поставить ряд дополнительных программ:
	*	[Microsoft .NET Framework 4.5.1](https://www.microsoft.com/ru-RU/download/details.aspx?id=40773). Он необходим для работы самой программы.
	*	Нужно установить один из драйверов перехвата пакета на выбор:
	*	[WinPcap 4.1.3](http://www.winpcap.org/install/bin/WinPcap_4_1_3.exe)
	*	или
	*	[WinpkFilter 3.2.3](http://www.ntkernel.com/downloads/winpkflt_rtl.zip)
2.	Скачиваем и распаковываем архив:
3.	Запускаем ”TeraModLoader.exe”:
4.	Выбираем драйвер перехвата.
5.	Выбираем сетевое соединение.
6.	Выбираем сервер.
7.	Выбираем версию протокола игры, сейчас это 29 04.
8.	Оставляем галочки на нужных модах.
9.	Запускаем игру Tera.
10.	Сверху окна должна появиться вкладка с соединением, значит вы всё сделали правильно.

## Вопросы и ответы

  [**Задать свой вопрос можно здесь!**](https://github.com/Detrav/TeraModLoader/issues)

## От автора

Сразу хочу сказать, что всем ПО вы пользуетесь на свой страх и риск. Если вас забанят, украдут личные данные, удалят персонажа, будут спамить голд в общий чат и т.п. — это не мои проблемы. Администрация написала, что пока программа будет только считывать данные, то бана не будет.

Я знаю 2 хороших варианта перехвата пакетов и оба они могут редактировать сетевой трафик: первый, клиент подключается к программе а программа к серверу, в итоге весь поток данных будет происходить через ПО, второй, драйвер пересылает программе пакеты сети, основываясь на фильтре, и ждёт в результате пакет назад.

В данной программе используется второй вариант, а это значит, что весь поток данных обрабатывается в программе и посылается назад, не факт, что программа ничего не сделала с потоком, она могла спокойно заменить пакет другой, а это вмешательство в игровой процесс.

###[Скачать с модами 0.7.14 от 1.7.15](https://github.com/Detrav/TeraModLoader/releases/download/v0.7.14-beta/TeraModLoader_v0_7_14.zip)
+Теперь не зависает
+Новый конфиг

Ещё пара слов, в плане ДПС и ХПС вычислить можно всё, но сложность вычисления близится к пику на дотах и магах. Сейчас не учитываются доты и некоторые скилы магов, это связано с тем, что у меня нету прокаченого мага до 65 лвла. Для подсчёта дотов, нужно писать больше механики, а времени у меня нет.

Позднее будет соло режим, т.е. считаться будет только для себя, там будет учитываться всё!
