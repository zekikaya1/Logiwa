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

 
``` CREATE DATABASE logiwa
    WITH OWNER admin;

create table category
(
    id           serial
        primary key,
    name         varchar(100)        not null,
    min_quantity integer   default 0 not null,
    created_date timestamp default CURRENT_TIMESTAMP,
    updated_date timestamp default CURRENT_TIMESTAMP,
    is_deleted   boolean   default false
);

alter table category
    owner to admin;

create index idx_category_name
    on category (name);

create index idx_category_is_deleted
    on category (is_deleted);

create table product
(
    id             serial
        primary key,
    name           varchar(200)        not null,
    description    text,
    stock_quantity integer   default 0 not null,
    category_id    integer
        constraint product_categoryid_fkey
            references category,
    created_date   timestamp default CURRENT_TIMESTAMP,
    updated_date   timestamp default CURRENT_TIMESTAMP,
    is_deleted     boolean   default false
);

alter table product
    owner to admin;

create index idx_product_category_id
    on product (category_id);

create index idx_product_is_deleted
    on product (is_deleted);

create index idx_product_stock_quantity
    on product (stock_quantity);

create index idx_product_name
    on product (name);


INSERT INTO public.category (id, name, min_quantity, created_date, updated_date, is_deleted) VALUES (1, 'Elektronik', 3, '2025-02-19 12:42:08.000000', '2025-02-19 12:42:10.000000', false);
INSERT INTO public.category (id, name, min_quantity, created_date, updated_date, is_deleted) VALUES (2, 'Giyim', 2, '2025-02-19 12:42:08.000000', '2025-02-19 12:42:10.000000', false);
INSERT INTO public.category (id, name, min_quantity, created_date, updated_date, is_deleted) VALUES (3, 'Oyuncak', 5, '2025-02-19 12:42:08.000000', '2025-02-19 12:42:10.000000', false);


INSERT INTO public.product (id, name, description, stock_quantity, category_id, created_date, updated_date, is_deleted) VALUES (1, 'Telefon', 'Telefon', 5, 1, '2025-02-21 11:18:33.935341', '2025-02-21 11:18:33.940261', false);
INSERT INTO public.product (id, name, description, stock_quantity, category_id, created_date, updated_date, is_deleted) VALUES (2, 'Elbise', 'elbise', 1, 2, '2025-02-21 11:18:49.026115', '2025-02-21 11:18:49.028012', false);
```

## Project Overview
The focus of this MVC project is primarily on the Product API. A layered architecture has not been implemented yet, as the primary objective is to work on the Product API.

Pending Tasks

Category Caching with Redis: To improve the response times, categories can be cached in Redis for 5 minutes. This will speed up subsequent requests for category data.

JWT Authentication for Product API: Adding JWT tokens to the Product API is recommended. This would provide secure authentication, ensuring that only authorized users can access the API. Even though this is an internal application, it's a good practice to implement authorization to protect sensitive data.

 









