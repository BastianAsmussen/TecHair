# TechHair Kravspecifikation

Denne kravspecifikation er lavet af en gruppe datateknikere med speciale i programmering fra Tech College. 
Formålet er at definere kravene til udviklingen af et omfattende system til frisøruddannelsen på vores skole. 
Systemet omfatter online booking, kundeadministration, PoS og backend API'er.
Kravene er opdelt i kategorier som website, PoS og API, med specifikke krav til både frontend og backend. 
Vi prioriterer disse krav for at sikre en effektiv udviklingsproces.
Gennem denne specifikation vil vi definere hvert krav detaljeret, 
fra online booking til MobilePay-betalinger og implementering af en lokal cache for PoS. 
Vi vil også diskutere behovet for konfigurationsfiler og sikkerhedsforanstaltninger som datarensning og API-autentificering.
Vores mål er at skabe et robust og brugervenligt system, der effektivt opfylder behovene i frisøruddannelsen.

Medlemmer: Bastian, Casper, Lukas og Zilas.

## Kategorier
- Website (W)
    - Frontend
    - Backend
- PoS (P)
    - Frontend
    - Backend
- API (A)
    - Database
    - Server
    - Endpoint

| ID  | Kategori | Krav                                                                                                                                                                                                                                      | Prioritering | Status  |
|-----|----------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------|---------|
| W1  | Backend  | Mulighed for at kunne bestille tider online med mulighed for at kunne vælge dato for klipning.                                                                                                                                            | Lav          |         |
| W2  | Backend  | Mulighed for at kunder kan oprette sig som brugere. Som bruger kan man browse tidligere klipninger.                                                                                                                                       | Lav          | Slettet |
| W3  | Backend  | Kunder skal kunne forudbetale når de bestiller tid hvis de ønsker.                                                                                                                                                                        | Lav          | Slettet |
| W4  | Backend  | Kunder skal have mulighed for at ændre en booket tid til ny dato, eller annulere den helt hvis nødvendigt.                                                                                                                                | Middel       |         |
| W5  | Backend  | Ansatte skal have et arbejdsskema så de kan se deres tidligere og kommende kundebehandlinger.                                                                                                                                             | Lav          |         |
| W6  | Backend  | Ansatte skal have mulighed for at ligge deres kommende tider i deres Outlook kalender så de kan modtage påmindelser.                                                                                                                      | Lav          |         |
| P1  | Frontend | Der skal være mulighed for at se dagens køb og de brugte produkter i PoS.<br><br>Dette skal vises i en tabel.                                                                                                                             | Middel       |         |
| P2  | Frontend | Der skal være mulighed for at danne sig et fuldt økonomisk overblik for dag, måned og år.<br><br>Dette skal vises som grafer.                                                                                                             | Lav          |         |
| W7  | Backend  | Lederen skal modtage advarsler eller påmindelser i.ft. ansattes fravær eller overskridelse af budgettet.                                                                                                                                  | Lav          | Slettet |
| A1  | Database | Opret en tabel over brugere ("users") som har følgende kolonner:<br>- id (PK)<br>- name<br>- email (U)<br>- password?                                                                                                                     | Høj          |         |
| A2  | Database | Opret en tabel over produkter ("products") med følgende kolonner:<br>- id (PK)<br>- name<br>- description<br>- price_id (FK -> price_history.id)<br>- stock                                                                               | Høj          |         |
| A3  | Database | Opret en tabel over ansatte ("employees") med følgende kolonner:<br>- id (PK)<br>- manager_id? (FK -> employees.id)<br>- user_id (FK -> users.id)                                                                                         | Høj          |         |
| A4  | Database | Opret en tabel over klipninger ("appointments") med følgende kolonner:<br>- id (PK)<br>- date<br>- price<br>- status<br>- notes<br>- client_id (FK -> users.id)<br>- barber_id (FK -> employees.id)<br>- hash (IDX)                       | Høj          |         |
| A5  | Database | Opret en tabel over salg ("sales") med følgende kolonner:<br>- id (PK)<br>- date<br>- appointment_id (FK -> appointments.id)<br>- product_id? (FK -> products.id)<br>- cashier_id (FK -> employees.id)                                    | Høj          |         |
| P3  | Backend  | Der skal kunne udregens total prisen på et salg. Dette kan opnås ved at læse fra tabellerne "appointments" og "products" for at udregne prisen på klipning og efterfølgende på produkter kunden køber med.                                | Høj          |         |
| P4  | Backend  | Efter udregning af pris skal kunden kunne scanne en QR kode med MobilePay for at betale den udregende pris.<br><br>Der laves en anmodning til MobilePays API hvori vi sætter vores QR kode pris til beløbet.                              | Middel       |         |
| P5  | Backend  | PoS skal have en lokal cache i tilfælde af at internettet går ned i en periode. Der skal laves i WAL som skrives til databasen igen når internetforbindelsen er genoprettet.                                                              | Høj          |         |
| A6  | Server   | Konfigurations filer med evne til at bestemme kirtiske variabler, f.eks. den mindst mulige tid mellem nu, og en ønsket tid til en booking.                                                                                                | Høj          |         |
| A7  | Server   | Backend API'en skal kunne pinge PoS'en i tilfælde af, at PoS'en har mistet forbindelse til internettet.                                                                                                                                   | Høj          |         |
| A8  | Server   | I tilfælde af at en bestilt tid er indenfor X antal tid, og PoS'en stadig mangler internet forbindelse så udsætter vi tiden med Y helt automatisk og underretter kunden via email.<br><br>(X og Y konfigurerbar via konfigurations filer) | Middel       |         |
| A9  | Server   | Sanitize data før brug.<br>- SQL Injection.<br>- Bad formatting.<br>- Email validation.                                                                                                                                                   | Høj          |         |
| P6  | Frontend | Se [PoS](#GUI##PoS) under [GUI](#GUI).                                                                                                                                                                                                    | Høj          |         |
| A10 | Database | Opret en tabel over salg ("price_history") med følgende kolonner:<br>- id (PK)<br>- date<br>- price                                                                                                                                       | Høj          |         |
| A11 | Endpoint | `/api/products/[CRUD]` => 2XX \| 5XX<br><br>"Authorization: API Key<br><br>Body:<br></product as JSON>"                                                                                                                                   | Høj          |         |
| A12 | Endpoint | `/api/appointments/[CRUD]` => 2XX \| 5XX<br><br>"Authorization: API Key<br><br>Body:<br></appointment as JSON>"                                                                                                                           | Høj          |         |
| A13 | Endpoint | `/api/users/[CRUD]` => 2XX \| 5XX<br><br>"Authorization: API Key<br><br>Body:<br></user as JSON>"                                                                                                                                         | Høj          |         |
| A14 | Endpoint | `/api/employees/[CRUD]` => 2XX \| 5XX<br><br>"Authorization: API Key<br><br>Body:<br></employee as JSON>"                                                                                                                                 | Høj          |         |

## GUI
### PoS
- TBD!
