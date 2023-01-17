CREATE TABLE "Leagues" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" text NULL,
    CONSTRAINT "PK_Leagues" PRIMARY KEY ("Id")
);

CREATE TABLE "Matches" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Date" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Matches" PRIMARY KEY ("Id")
);

CREATE TABLE "Teams" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" text NULL,
    "LeagueId" integer NOT NULL,
    CONSTRAINT "PK_Teams" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Teams_Leagues_LeagueId" FOREIGN KEY ("LeagueId") REFERENCES "Leagues" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Coaches" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" text NULL,
    "TeamId" integer NOT NULL,
    CONSTRAINT "PK_Coaches" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Coaches_Teams_TeamId" FOREIGN KEY ("TeamId") REFERENCES "Teams" ("Id") ON DELETE CASCADE
);

CREATE TABLE "TeamMatches" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "TeamId" integer NOT NULL,
    "MatchId" integer NOT NULL,
    CONSTRAINT "PK_TeamMatches" PRIMARY KEY ("Id", "TeamId", "MatchId"),
    CONSTRAINT "FK_TeamMatches_Matches_MatchId" FOREIGN KEY ("MatchId") REFERENCES "Matches" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_TeamMatches_Teams_TeamId" FOREIGN KEY ("TeamId") REFERENCES "Teams" ("Id") ON DELETE RESTRICT
);

CREATE UNIQUE INDEX "IX_Coaches_TeamId" ON "Coaches" ("TeamId");
CREATE INDEX "IX_TeamMatches_MatchId" ON "TeamMatches" ("MatchId");
CREATE INDEX "IX_TeamMatches_TeamId" ON "TeamMatches" ("TeamId");
CREATE INDEX "IX_Teams_LeagueId" ON "Teams" ("LeagueId");


INSERT INTO public."Leagues"("Name") VALUES ('La Liga');
INSERT INTO public."Leagues"("Name") VALUES ('Ligue 1');
INSERT INTO public."Leagues"("Name") VALUES ('Premier League');

INSERT INTO public."Teams"("Name", "LeagueId") VALUES ('Real Madrid', 1);
INSERT INTO public."Teams"("Name", "LeagueId") VALUES ('Lille', 2);
INSERT INTO public."Teams"("Name", "LeagueId") VALUES ('Manchester City', 3);

SELECT * FROM public."Leagues";