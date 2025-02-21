# Logiwa


Ensure the following dependencies are installed before setting up the project:

- **Docker** (Required for running PostgreSQL, Redis, and Elasticsearch containers)
- **.NET 8 SDK** (Required for running the application locally) 

## Installation & Setup

### 1. Clone the Repository
bash
git clone https://github.com/zekikaya1/logiwa-product-api.git
cd logiwa-product-api


## API Endpoints
``` Products API
Method	Endpoint	Description
GET	/api/products	Get all products
GET	/api/products/{id}	Get product by ID
POST	/api/products	Create a new product
PUT	/api/products/{id}	Update a product
DELETE	/api/products/{id}	Delete a product
GET	/api/products/search	Search products by filters

## Categories API
Method	Endpoint	Description
GET	/api/categories	Get all categories
GET	/api/categories/{id}	Get category by ID

 

## 1. Create Database

 
``` CREATE DATABASE database_name
    WITH OWNER admin;

2. Create Category Table
sql
Kopyala
CREATE TABLE category
(
    id           SERIAL PRIMARY KEY,
    name         VARCHAR(100)        NOT NULL,
    min_quantity INTEGER   DEFAULT 0 NOT NULL,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_deleted   BOOLEAN   DEFAULT FALSE
);
Alter Table Category
sql
Kopyala
ALTER TABLE category
    OWNER TO admin;
Create Indexes for Category
sql
Kopyala
CREATE INDEX idx_category_name
    ON category (name);

CREATE INDEX idx_category_is_deleted
    ON category (is_deleted);
3. Create Product Table
sql
Kopyala
CREATE TABLE product
(
    id             SERIAL PRIMARY KEY,
    name           VARCHAR(200)        NOT NULL,
    description    TEXT,
    stock_quantity INTEGER   DEFAULT 0 NOT NULL,
    category_id    INTEGER
        CONSTRAINT product_categoryid_fkey
            REFERENCES category,
    created_date   TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_date   TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_deleted     BOOLEAN   DEFAULT FALSE
);
Alter Table Product
sql
Kopyala
ALTER TABLE product
    OWNER TO admin;
Create Indexes for Product
sql
Kopyala
CREATE INDEX idx_product_category_id
    ON product (category_id);

CREATE INDEX idx_product_is_deleted
    ON product (is_deleted);

CREATE INDEX idx_product_stock_quantity
    ON product (stock_quantity);

CREATE INDEX idx_product_name
    ON product (name); 



