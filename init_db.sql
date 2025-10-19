-- =====================================================
-- Database initialization script for Employee Web API
-- =====================================================

DROP TABLE IF EXISTS passports CASCADE;
DROP TABLE IF EXISTS employees CASCADE;
DROP TABLE IF EXISTS departments CASCADE;
DROP TABLE IF EXISTS companies CASCADE;

CREATE TABLE companies (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL
);

CREATE TABLE departments (
    id SERIAL PRIMARY KEY,
    company_id INT NOT NULL REFERENCES companies(id) ON DELETE CASCADE,
    name TEXT NOT NULL,
    phone TEXT NULL,
    UNIQUE (company_id, name)
);

CREATE TABLE employees (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    surname TEXT NOT NULL,
    phone TEXT NULL,
    company_id INT NOT NULL REFERENCES companies(id) ON DELETE CASCADE,
    department_id INT NULL REFERENCES departments(id) ON DELETE SET NULL
);

CREATE TABLE passports (
    id SERIAL PRIMARY KEY,
    employee_id INT NOT NULL UNIQUE REFERENCES employees(id) ON DELETE CASCADE,
    type TEXT NULL,
    number TEXT NULL
);


INSERT INTO companies (name) VALUES ('Smartway'), ('Google');

INSERT INTO departments (company_id, name, phone)
VALUES 
    (1, 'Dev', '111-222'),
    (1, 'HR', '111-333'),
    (2, 'Support', '222-444');

INSERT INTO employees (name, surname, phone, company_id, department_id)
VALUES 
    ('John', 'Doe', '555-123', 1, 1),
    ('Jane', 'Smith', '555-456', 1, 2),
    ('Mike', 'Brown', '555-789', 2, 3);

INSERT INTO passports (employee_id, type, number)
VALUES 
    (1, 'Internal', 'AA123456'),
    (2, 'Foreign', 'BB654321');
