- [Tutorial](https://www.youtube.com/watch?v=q2AcJmB03Io&list=PL82C6-O4XrHde_urqhKJHH-HTUfTK6siO)
- [Tutorial 2](https://www.youtube.com/watch?v=ZypiARkybY0)

### Migration Commands

- Intial Migration

```powershell
dotnet ef migrations add InitialCreate
```

- Remove All Migration and start from scratch

```powershell
dotnet ef database update 0
dotnet ef migrations remove
```

- Remove Last Migration

```powershell
dotnet ef migrations remove
```

- Remove Specific Migration

```powershell
dotnet ef database update <MigrationName>
```

- Running the project with Hot reload

```powershell
dotnet watch run
```
## TO DO
[ ] use htmx instead of ajax