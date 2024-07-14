# LearningAuthenticationAndAuthorization
#### Topic covered ####

##### Learned authentication and authorization using JWT Tokens ######
  #### Packages ####
  Microsoft.AspNetCore.Authentication.JwtBearer
  #### Steps ####
  1. Created login controller and added login method to authenticate the user and generate JWT token, and added refresh method to new generate JWT token <a href="https://github.com/RamadossE2313/LearningAuthenticationAndAuthorization/blob/main/Controllers/LoginController.cs">Login controller </a>
  2. When we get token we have to authenticate token is valid or not, received correct issuer and audidence for that, we have to add AddAuthentication in the configure service section
     <img width="733" alt="image" src="https://github.com/user-attachments/assets/01bf17d9-36f1-4249-b7c7-b259301df5e3">
  3. We should add AddAuthorization in the configuration section
  4. We should add UseAuthentication and UseAuthorization in the configure method to get authenticated and authorization validated in the request
  
  #### Here we have used only JWT authenticaion scheme, so file name mentioned as "SingleAuthenticationSchemaSchema.cs" <a href="https://github.com/RamadossE2313/LearningAuthenticationAndAuthorization/blob/main/SingleAuthenticationSchemaSchema.cs">SingleAuthenticationSchemaSchema</a> #####
  #### Here we have used JWT and Cookie authentication schema, so file name mentioned as "MultipleAuthenticationSchema.cs" <a href="https://github.com/RamadossE2313/LearningAuthenticationAndAuthorization/blob/main/MultipleAuthenticationSchema.cs">MultipleAuthenticationSchema</a> #####
  to differenciate with SingleAuthenticationSchemaSchema and MultipleAuthenticationSchema, created versioning for the controllers.

  ### References ###
  **Blog**: https://code-maze.com/dotnet-multiple-authentication-schemes/ </hr>
  **Youtube**: https://www.youtube.com/watch?v=dsuPRZ6V9Xg </hr>
  **MultipleAuthenticationSchema in Minimal API**: https://github.com/Anish407/AuthorizeUsingMultipleSchemes/tree/master

