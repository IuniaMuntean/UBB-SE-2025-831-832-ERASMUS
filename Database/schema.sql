
-- Create main schema
CREATE SCHEMA transport;

-- Companies table
CREATE TABLE transport.companies (
    company_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    address TEXT NOT NULL,
    phone VARCHAR(20),
    email VARCHAR(100)
);

-- Drivers table
CREATE TABLE transport.drivers (
    driver_id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    license_number VARCHAR(20) UNIQUE NOT NULL,
    phone VARCHAR(20),
    email VARCHAR(100),
    hire_date DATE NOT NULL,
    status VARCHAR(15) NOT NULL CHECK (status IN ('ACTIVE', 'INACTIVE', 'VACATION', 'SICK_LEAVE'))
);

-- Trucks table
CREATE TABLE transport.trucks (
    truck_id SERIAL PRIMARY KEY,
    license_plate VARCHAR(15) UNIQUE NOT NULL,
    make VARCHAR(30) NOT NULL,
    model VARCHAR(30) NOT NULL,
    year INTEGER NOT NULL,
    capacity_kg DECIMAL(10, 2) NOT NULL,
    status VARCHAR(15) NOT NULL CHECK (status IN ('AVAILABLE', 'IN_MAINTENANCE', 'IN_USE', 'RETIRED')),
    last_maintenance_date DATE,
    next_maintenance_date DATE
);

-- Deliveries table
CREATE TABLE transport.deliveries (
    delivery_id SERIAL PRIMARY KEY,
    reference_number VARCHAR(20) UNIQUE NOT NULL,
    departure_address TEXT NOT NULL,
    destination_address TEXT NOT NULL,
    departure_time TIMESTAMP WITH TIME ZONE NOT NULL,
    estimated_time_arrival TIMESTAMP WITH TIME ZONE NOT NULL,
    status VARCHAR(20) NOT NULL CHECK (status IN ('PLANNED', 'IN_PROGRESS', 'COMPLETED', 'CANCELLED')),
    driver_id INTEGER REFERENCES transport.drivers(driver_id),
    truck_id INTEGER REFERENCES transport.trucks(truck_id),
    company_id INTEGER REFERENCES transport.companies(company_id),
    cargo_description TEXT,
    weight_kg DECIMAL(10, 2),
    total_distance_km DECIMAL(10, 2),
    fee_euros DECIMAL(10, 2)
);

CREATE TABLE transport.users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE transport.orders (
    order_id SERIAL PRIMARY KEY,
    client_name VARCHAR(255) NOT NULL,
    cargo_type VARCHAR(255) NOT NULL,
    cargo_weight DOUBLE PRECISION NOT NULL,
    source_city VARCHAR(255) NOT NULL,
    destination_city VARCHAR(255) NOT NULL
);



