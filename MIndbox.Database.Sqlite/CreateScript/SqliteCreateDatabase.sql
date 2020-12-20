CREATE TABLE figures (
    id   INTEGER PRIMARY KEY AUTOINCREMENT
                 NOT NULL,
    type INT     CONSTRAINT TypeFigureConstraint CHECK (type = 1 OR 
                                                        type = 2) 
                 NOT NULL,
    area DOUBLE  CONSTRAINT [Area greater than or equal to zero] CHECK (area >= 0) 
                 NOT NULL
);


CREATE TABLE circles (
    idcircle INTEGER PRIMARY KEY
                     REFERENCES figures (id) ON UPDATE NO ACTION
                                             MATCH [FULL]
                     NOT NULL,
    radius   DOUBLE  NOT NULL
);

CREATE TABLE triangle (
    id INTEGER PRIMARY KEY
               REFERENCES figures (id) ON UPDATE CASCADE
                                       MATCH [FULL]
               NOT NULL,
    a  DOUBLE  NOT NULL,
    b  DOUBLE  NOT NULL,
    c  DOUBLE  NOT NULL
);

