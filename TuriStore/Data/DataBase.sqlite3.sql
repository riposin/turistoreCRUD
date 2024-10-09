PRAGMA encoding = "UTF-8"; 
.open turistore.db.sqlite3

-- -- -- -- NOTES -- -- -- --
-- NET C# types:
---- BLOB (n)       -> byte[36]
---- NVARCHAR(n)    -> string
---- REAL           -> double
---- NUMERIC        -> decimal
---- INTEGER        -> Int64(long)

-- -- -- -- TABLES -- -- -- --

CREATE TABLE items (
    
    guid        BLOB (16)      NOT NULL
                               PRIMARY KEY,
    sku         NVARCHAR (12)  NOT NULL,
    description NVARCHAR (200),
    tax         INTEGER           NOT NULL,
    unit_price  REAL           NOT NULL,
    existence   INTEGER        NOT NULL,
    UNIQUE (
        sku
    )
);



-- -- -- -- DEFAULT CONFIG -- -- -- --
-- -- -- -- LABELS -- -- -- --
-- -- -- -- TRIGGERS -- -- -- --

.quit