CREATE TABLE figures (
    id   INTEGER PRIMARY KEY AUTOINCREMENT,
    area DOUBLE  CONSTRAINT [Area greater than or equal to zero] CHECK (area >= 0) 
                 NOT NULL
);