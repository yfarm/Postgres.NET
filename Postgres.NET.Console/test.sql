--
-- PostgreSQL database dump
--

-- Dumped from database version 9.5.1
-- Dumped by pg_dump version 9.5.1

-- Started on 2016-09-26 06:40:29

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 1 (class 3079 OID 12355)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2104 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 181 (class 1259 OID 156572)
-- Name: person; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE person (
    id integer NOT NULL,
    name text NOT NULL,
    age integer,
    employed boolean NOT NULL,
    created_date timestamp with time zone NOT NULL
);


ALTER TABLE person OWNER TO postgres;

--
-- TOC entry 182 (class 1255 OID 156583)
-- Name: func_get_persons(text[]); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION func_get_persons(_name text[]) RETURNS SETOF person
    LANGUAGE plpgsql COST 1 ROWS 10
    AS $$
DECLARE
BEGIN
  return query
  select p.* from person p
  where p.name = ANY(_name);
  END;
$$;


ALTER FUNCTION public.func_get_persons(_name text[]) OWNER TO postgres;

--
-- TOC entry 1982 (class 2606 OID 156579)
-- Name: id_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY person
    ADD CONSTRAINT id_pkey PRIMARY KEY (id);


--
-- TOC entry 2103 (class 0 OID 0)
-- Dependencies: 6
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2016-09-26 06:40:29

--
-- PostgreSQL database dump complete
--

