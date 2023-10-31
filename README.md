# Cooking Media Authentication

## 1. Database Migration
### 1.1. Add secrets
- Option 1:
  - Right click on the *CookingMedia.Authentication* project. Select *Manage User Secrets*.
  - Update the *secrets.json* file:
    ```json
	{
	  "ConnectionStrings:DefaultConnection": "Server=(local);Database=CookingMedia_Authentication;Uid=your-user-id;Pwd=your-password;TrustServerCertificate=true",
	  "Jwt:Key": "A key used for generating JWT"
	}
	```
- Option 2: Run the following commands in *CookingMedia.Authentication* project:
  ```bash
  > dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(local);Database=CookingMedia_Authentication;Uid=your-user-id;Pwd=your-password;TrustServerCertificate=true",
  > dotnet user-secrets set "Jwt:Key" "A key used for generating JWT"
  ```

### 1.2. Migration
If there is changes in entities, generate a migration by running the command below in the *CookingMedia.Authentication* project:
```bash
dotnet ef migrations add migration-name
```

### 1.3. Update
To update the database or creat database after cloning the repo, run the command below in the *CookingMedia.Authentication* project:
```bash
dotnet ef database update
```
