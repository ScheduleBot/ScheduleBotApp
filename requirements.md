# Code Fellows .NET-401d4 Final Project Group 2 

# ToDoBoT Software Requirements 

## Vision 

TDBT uses Azure Bot Services to provide a natural language interface for a planning application. TDBT will allow users to quickly and easily manage their schedules and set reminders for themselves. Our vision is to make navigating our oftentimes hectic everyday lives as painless as possible. 

##Scope (In/Out)

###In 

1. A user can interact with the bot.

2. Bot can retrieve task list from database 

3. Bot can display task list to channel 

4. Admin can Create, Read, Update or Delete tasks through front-end  

5. Tasks are stored in a database 

###Out 

1. The bot cannot tell the future 

##Minimum Viable Product 

1. Admin can CRUD tasks in database 

2. Chatbot can access database 

3. Chatbot can find database tasks 

4. Chatbot can display database tasks to channel 

##Stretch Goals 

1. Speech-to-text capability from user to bot. 

2. Text-to-speech capability from bot to user. 

3. User can add items to unique todo list by talking to the bot 

4. Bot will be able to identify unique users 

5. Bot will be able to create, update, or delete tasks for unique users 

##Functional Requirements 

1. Users can create accounts 

2. Users can create an event 

3. Users can perform CRUD on their events 

4. Bot can access database 

5. Bot can display tasks 

##Non-Functional Requirements 

1. Security 

	a. Code base is securely hosted on Azure and Github 

	b. HTTPS enabled for test site. 

	c. Bot deployed on secured chat platforms. 

	d. User is not able to inject malware through bot interaction. 

	e. Identity validation of users on site login. 

	f. Admin role authorization limits access to admin function. 

2. Testability 

	a. 90% code coverage through XUnit testing. 

##Data Flow 

1. The admin logs into the front end site. 

2. The admin can CRUD database entries. 

3. The user enters the chat room. 

4. The user can interact with the bot by greeting. 

5. If no user interaction, the bot activates at certain times. 

6. The bot retrieves task list from the database. 

7. The bot displays task list in the chat program. 