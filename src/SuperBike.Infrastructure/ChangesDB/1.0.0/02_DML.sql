/*Inserção de perfis*/
insert into "AspNetRoles" values 
(
	'cea7c7dc-6934-4929-bd60-59cc8df3c18d', 
	'RenterDeliveryman', 
	'RENTERDELIVERYMAN', 
	null
);

insert into "AspNetRoles" values 
(
	'96abc0ec-c501-49a9-8d2d-321492250da8', 
	'Admin', 
	'ADMIN', 
	null
);

/*Inserção de Usuário 'Admin' {"loginUserName": "admin@email.com", "password": "Admin*123456"}*/
insert into "AspNetUsers" values 
(
	'bf834e46-b77a-4ea2-ba67-719aebb55a4a',
	'admin@email.com',
	'ADMIN@EMAIL.COM',
	'admin@email.com',
	'ADMIN@EMAIL.COM',
	true,
	'AQAAAAIAAYagAAAAENK6Q4N4DaDkTVWPQmYdU4pToyVJNTGfJARYHOLsvwtQCp9ffEbVgEdW+XpuKWKXrQ==',
	'PKRLNVNRVSP4SG4A6B5EFOA37G7NDG5K',
	'f4f1053b-ab6b-4643-9b1c-bbc318f3fa8d',
	NULL,
	false,
	false,
	NULL,
	false,
	0
);

insert into "AspNetUserRoles" values
(
	'bf834e46-b77a-4ea2-ba67-719aebb55a4a', /*Usuário 'Admin'*/
	'96abc0ec-c501-49a9-8d2d-321492250da8'  /*Role Admin*/
);

/*Hitorico de migrações*/
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('00000000000000_Initial', '8.0.6');

/*Script de criação de tabelas SuperBike*/
insert into rentalplan (days, valueperday, percentageofdailynoteffectived, valueperdayexceeded) values (7, 30, 0.2, 50);
insert into rentalplan (days, valueperday, percentageofdailynoteffectived, valueperdayexceeded) values (15, 28, 0.4, 50);
insert into rentalplan (days, valueperday, percentageofdailynoteffectived, valueperdayexceeded) values (30, 22, 0.0, 50);
insert into rentalplan (days, valueperday, percentageofdailynoteffectived, valueperdayexceeded) values (45, 20, 0.0, 50);
insert into rentalplan (days, valueperday, percentageofdailynoteffectived, valueperdayexceeded) values (50, 18, 0.0, 50);