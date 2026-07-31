# IT Asset Management System

A desktop application for IT departments to track company equipment: what exists, who has it, where it is, its warranty status, its repair history, and the software licenses tied to it.

Built with **C#**, **.NET 8**, **WPF**, and **Supabase** (PostgreSQL). Each company connects the app to its own Supabase project — there is no shared backend and no separate paid service to run.

## Features

**Implemented**
- First-time setup screen to connect the app to a Supabase project (project URL + publishable key), verified live before saving
- Add, edit, and view equipment (laptops, desktops, monitors, phones, tablets, and more)
- Automatic asset ID generation alongside a company asset tag (e.g. `LAP-0042`)
- Dashboard with live stats (total / available / assigned / in-repair) and warranty-expiration warnings, backed by real Supabase data
- Light/dark theme and adjustable UI scale, from the Settings page
- Disconnect / switch database from Settings, without losing your saved connection
- Maintenance/repair record tracking (service date, cost, technician, status) at the data layer

**Planned**
- Assignment, return, and transfer workflow with automatic status updates
- Software license tracking and seat assignment
- Reports and CSV/Excel export
- Search and filtering across all asset fields

## Tech Stack

| Layer | Technology |
|---|---|
| UI | WPF (.NET 8, Windows) |
| Language | C# |
| Database | Supabase (PostgreSQL) |
| DB Client | `supabase-csharp` |
| Charts | LiveCharts *(planned)* |
| Reports | ClosedXML *(planned)* |

## Prerequisites

- Windows 10/11
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 (with the **.NET desktop development** workload) or the `dotnet` CLI
- A free [Supabase](https://supabase.com) account

## Setup

### 1. Clone the repository

```bash
git clone https://github.com/tarekhamrouni/IT-Asset-Management-System.git
cd IT-Asset-Management-System
```

### 2. Create a Supabase project

1. Go to [supabase.com](https://supabase.com) and create a new project.
2. Once it's ready, open the **SQL Editor** and run the schema below to create the required tables and security policies.

<details>
<summary><strong>Database schema (click to expand)</strong></summary>

```sql
create extension if not exists "pgcrypto";

create or replace function set_updated_at()
returns trigger as $$
begin
  new.updated_at = now();
  return new;
end;
$$ language plpgsql;

-- Assets
create table assets (
  id uuid primary key default gen_random_uuid(),
  asset_tag text not null unique,
  type text not null check (type in (
    'Laptop', 'Desktop', 'Monitor', 'MobilePhone', 'Tablet',
    'Keyboard', 'Headset', 'Printer', 'Server', 'NetworkDevice'
  )),
  brand text not null default '',
  model text not null default '',
  serial_number text not null default '',
  status text not null default 'Available' check (status in (
    'Available', 'Assigned', 'InRepair', 'Broken', 'Lost', 'Retired'
  )),
  assigned_to text,
  department text,
  purchase_date date,
  warranty_expires date,
  location text,
  created_at timestamptz not null default now(),
  updated_at timestamptz not null default now()
);

create index idx_assets_type on assets (type);
create index idx_assets_status on assets (status);
create index idx_assets_serial_number on assets (serial_number);
create index idx_assets_warranty_expires on assets (warranty_expires);

create trigger trg_assets_updated_at
  before update on assets
  for each row execute function set_updated_at();

-- Assignment / return / transfer history
create table asset_assignments (
  id uuid primary key default gen_random_uuid(),
  asset_id uuid not null references assets (id) on delete cascade,
  assigned_to text not null,
  department text,
  assignment_date date not null default current_date,
  return_date date,
  notes text,
  created_at timestamptz not null default now()
);

create index idx_asset_assignments_asset_id on asset_assignments (asset_id);
create index idx_asset_assignments_assigned_to on asset_assignments (assigned_to);

create unique index idx_asset_assignments_open_per_asset
  on asset_assignments (asset_id)
  where return_date is null;

-- Maintenance / repair history
create table maintenance_records (
  id uuid primary key default gen_random_uuid(),
  asset_id uuid not null references assets (id) on delete cascade,
  service_date date not null default current_date,
  description text not null default '',
  cost numeric(12, 2),
  technician text,
  repair_status text not null default 'Pending' check (repair_status in (
    'Pending', 'InProgress', 'Completed', 'Cancelled'
  )),
  notes text,
  created_at timestamptz not null default now()
);

create index idx_maintenance_records_asset_id on maintenance_records (asset_id);
create index idx_maintenance_records_service_date on maintenance_records (service_date);

-- Software licenses
create table software_licenses (
  id uuid primary key default gen_random_uuid(),
  license_name text not null,
  license_key text,
  total_seats integer not null default 1,
  available_seats integer not null default 1,
  expiration_date date,
  notes text,
  created_at timestamptz not null default now(),
  updated_at timestamptz not null default now(),
  constraint chk_license_seats check (available_seats >= 0 and available_seats <= total_seats)
);

create index idx_software_licenses_expiration on software_licenses (expiration_date);

create trigger trg_software_licenses_updated_at
  before update on software_licenses
  for each row execute function set_updated_at();

-- Software license assignments
create table license_assignments (
  id uuid primary key default gen_random_uuid(),
  license_id uuid not null references software_licenses (id) on delete cascade,
  assigned_to text not null,
  department text,
  assignment_date date not null default current_date,
  return_date date,
  notes text,
  created_at timestamptz not null default now()
);

create index idx_license_assignments_license_id on license_assignments (license_id);

create unique index idx_license_assignments_open_per_person
  on license_assignments (license_id, assigned_to)
  where return_date is null;

-- Row Level Security
-- The app has no authentication in this version and connects using only the
-- publishable (anon) key, so access is scoped to "who holds the project's key."
alter table assets enable row level security;
alter table asset_assignments enable row level security;
alter table maintenance_records enable row level security;
alter table software_licenses enable row level security;
alter table license_assignments enable row level security;

create policy "anon full access" on assets
  for all to anon using (true) with check (true);

create policy "anon full access" on asset_assignments
  for all to anon using (true) with check (true);

create policy "anon full access" on maintenance_records
  for all to anon using (true) with check (true);

create policy "anon full access" on software_licenses
  for all to anon using (true) with check (true);

create policy "anon full access" on license_assignments
  for all to anon using (true) with check (true);
```

</details>

### 3. Get your project credentials

In your Supabase project, go to **Settings → API** and copy:
- **Project URL**
- **Publishable key** (also called the anon/public key)

> Never use the **service role** key in this app — only the publishable key, which is safe to use client-side because access is controlled by the Row Level Security policies above.

### 4. Run the app

Using Visual Studio:
1. Open `IT Asset Management System.slnx`
2. Press **F5** to build and run

Using the .NET CLI:
```bash
dotnet run --project "IT Asset Management System/IT Asset Management System.csproj"
```

### 5. Connect to your database

On first launch, the app will show a setup screen. Enter your Supabase **Project URL** and **Publishable key**, then continue — the app verifies the connection before saving it locally (`%AppData%\IT Asset Management System\config.json`) and loading your data.

You can change or disconnect the database connection later from **Settings**.

## Project Structure

```
IT Asset Management System/
├── Config/          # Local app configuration model
├── Models/          # Data models mapped to Supabase tables
├── Services/        # Supabase connection, config storage, and CRUD services
├── Views/           # WPF pages and windows
└── App.xaml         # Application entry point and shared theme resources
```

## Team

| Name | Focus |
|---|---|
| Tarek Hamrouni | Database design, Supabase configuration, data models, backend services |
| Ojee Said | WPF interface, pages, forms, navigation, styling |
