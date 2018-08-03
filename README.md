# Introduction 
ScheduleBot uses Azure Bot Services to provide a natural language interface for a planning application. ScheduleBot will allow users to quickly and easily manage their schedules and set reminders for themselves. Our vision is to make navigating our oftentimes hectic everyday lives as painless as possible.  
[Requirements Document](/Requirements.md)

## Wireframes
![Wireframe](/AllWireFrames.PNG)

## Database Schema
![Database Schema](/DBSchema.png)
The database will have two tables: a table of schedules, and a table of schedule items. Each schedule in the schedules table will have an ID, a foreign key pointing to the user that owns it, and the schedule's name. Each schedule item contains an ID, a foreign key pointing to the schedule it belongs to, the title of the item, the day that the event takes place (if the schedule item does not repeat), the time of day the event starts, how long the event will last, and a set of flags determining which days (if any) the event will occur on each week.