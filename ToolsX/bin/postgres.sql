-- Table: public.tbl_toolx_tips

-- DROP TABLE IF EXISTS public.tbl_toolx_tips;

CREATE TABLE IF NOT EXISTS public.tbl_toolx_tips
(
    "SHEETID" text COLLATE pg_catalog."default" NOT NULL,
    "SOLUTION" text COLLATE pg_catalog."default" NOT NULL,
    "FILEPATH" text COLLATE pg_catalog."default" NOT NULL,
    "DESCRIPTION" text COLLATE pg_catalog."default" NOT NULL,
    "USER" text COLLATE pg_catalog."default" NOT NULL,
    "STATUS" text COLLATE pg_catalog."default" NOT NULL DEFAULT 0,
    "KEYSTRING" text COLLATE pg_catalog."default" NOT NULL,
    "TYPE" text COLLATE pg_catalog."default" DEFAULT 'tip'::text,
    CONSTRAINT "TBL_TOOLX_TIPS_pkey" PRIMARY KEY ("SHEETID")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.tbl_toolx_tips
    OWNER to postgres;