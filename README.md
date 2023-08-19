# FoodRecipesSolution

Beskrivning 

Uppgiften handlar om att skapa en databas med möjlighet att lagra olika matrecept. All 
information skall sparas i en databas som du själv skapar. Det skall finnas ett web api som 
skapas med ASP.NET web api, genom vilket det finns möjlighet att kommunicera med 
databasen (via Dapper) . Krav för att uppgiften skall bli godkänd: 
- Det skall gå att lägga upp ett användarkonto med användarnamn, lösenord och email. 
En användare skall kunna logga in. Det skall även gå att uppdatera samt ta bort ett 
användarkonto. När en användare loggar in skall användarid returneras och detta skall 
sedan användas i andra anrop.
- En inloggad användare skall kunna lägga upp recept. Då skickas användarens id med i 
anropet och receptet kopplas till denna användare . Följande information skall kunna 
registreras och sparas för ett recept: titel, beskrivning, ingredienser och kategori (typ 
av recept). 
- Det skall gå att ange vilken kategori av recept det är. Exempel på kategorier kan vara 
Svensk husmanskost, Franskt, Italienskt, Kinesiskt. Giltiga värden skall vara lagrade i 
en egen tabell i databasen. 
- Det skall gå att spara ett nytt recept i databasen men också uppdatera ett recept som 
redan finns. Det skall även gå att ta bort ett recept och det skall då försvinna från 
databasen. Bara den användare som skapat receptet skall kunna ta bort eller uppdatera 
det. 
- En användare kan betygssätta andra användares recept (Rating) på en skala från 1-5. 
Man skall inte kunna betygssätta sina egna recept. 
- Det skall gå att söka på recept på titel. Sökningen skall fungera som ett urval på titel 
och få träffar på de recept som matchar sökvillkoret. 
