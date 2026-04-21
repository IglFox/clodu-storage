<template>
    <div class="dashboard-container">
        <aside class="sidebar">
            <div class="sidebar-header">
                <img
                    src="/path/to/parlay-banditz-logo.svg"
                    alt="Parlay Banditz Logo"
                    class="logo"
                />
                <span class="brand-name">Parlay Banditz</span>
                <button class="settings-button" aria-label="Settings">
                    <svg
                        width="16"
                        height="16"
                        viewBox="0 0 16 16"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                    >
                        <path
                            d="M7.99992 14.6667C11.6666 14.6667 14.6666 11.6667 14.6666 8.00004C14.6666 4.33337 11.6666 1.33337 7.99992 1.33337C4.33325 1.33337 1.33325 4.33337 1.33325 8.00004C1.33325 11.6667 4.33325 14.6667 7.99992 14.6667Z"
                            stroke="currentColor"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path
                            d="M8 4.66663V11.3333"
                            stroke="currentColor"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path
                            d="M4.66675 8H11.3334"
                            stroke="currentColor"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                    </svg>
                </button>
            </div>
            <nav class="sidebar-nav">
                <ul>
                    <li class="nav-item active">
                        <a href="#" class="nav-link">
                            <svg
                                class="icon"
                                width="20"
                                height="20"
                                viewBox="0 0 20 20"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    d="M17.5 10.8333H15V17.5H5V10.8333H2.5L10 3.33333L17.5 10.8333Z"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M5 17.5V10.8333"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M15 17.5V10.8333"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                            Home
                        </a>
                    </li>
                    <li class="nav-item has-submenu">
                        <a href="#" class="nav-link">
                            <svg
                                class="icon"
                                width="20"
                                height="20"
                                viewBox="0 0 20 20"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    d="M18.3333 5.83333V17.5H5V5.83333H18.3333Z"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M18.3333 5.83333L10 10.8333L1.66667 5.83333"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M5 5.83333H18.3333"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                            Store
                        </a>
                        <ul class="submenu">
                            <li><a href="#">Store Editor</a></li>
                            <li><a href="#">Products</a></li>
                            <li><a href="#">Customers</a></li>
                        </ul>
                    </li>
                    <li class="nav-item has-submenu">
                        <a href="#" class="nav-link">
                            <svg
                                class="icon"
                                width="20"
                                height="20"
                                viewBox="0 0 20 20"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    d="M17.5 14.1667H15V17.5H5V14.1667H2.5L10 6.66667L17.5 14.1667Z"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M5 17.5V14.1667"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M15 17.5V14.1667"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                            Apps
                        </a>
                        <ul class="submenu">
                            <li><a href="#">Browse</a></li>
                            <li><a href="#">Installed</a></li>
                        </ul>
                    </li>
                    <li class="nav-item has-submenu">
                        <a href="#" class="nav-link">
                            <svg
                                class="icon"
                                width="20"
                                height="20"
                                viewBox="0 0 20 20"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    d="M17.5 12.5V17.5H2.5V12.5H5L7.5 10L10 12.5H12.5L15 10L17.5 12.5Z"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M5 17.5V12.5"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M15 17.5V12.5"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                            Marketing
                        </a>
                        <ul class="submenu">
                            <li><a href="#">Affiliates</a></li>
                            <li><a href="#">Links</a></li>
                            <li><a href="#">Promo codes</a></li>
                            <li><a href="#">Events</a></li>
                        </ul>
                    </li>
                    <li class="nav-item has-submenu">
                        <a href="#" class="nav-link">
                            <svg
                                class="icon"
                                width="20"
                                height="20"
                                viewBox="0 0 20 20"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    d="M17.5 15H15V17.5H5V15H2.5L10 7.5L17.5 15Z"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M5 17.5V15"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M15 17.5V15"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                            Finances
                        </a>
                        <ul class="submenu">
                            <li><a href="#">Payments</a></li>
                            <li><a href="#">Payouts</a></li>
                            <li><a href="#">Billing history</a></li>
                        </ul>
                    </li>
                    <li class="nav-item">
                        <a href="#" class="nav-link">
                            <svg
                                class="icon"
                                width="20"
                                height="20"
                                viewBox="0 0 20 20"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    d="M17.5 15.8333H15V17.5H5V15.8333H2.5L10 8.33333L17.5 15.8333Z"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M5 17.5V15.8333"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M15 17.5V15.8333"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                            Feedback
                        </a>
                    </li>
                    <li class="nav-item">
                        <a href="#" class="nav-link">
                            <span class="badge">5</span>
                            <svg
                                class="icon"
                                width="20"
                                height="20"
                                viewBox="0 0 20 20"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    d="M17.5 15.8333H15V17.5H5V15.8333H2.5L10 8.33333L17.5 15.8333Z"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M5 17.5V15.8333"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M15 17.5V15.8333"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                            Resolution center
                        </a>
                    </li>
                </ul>
            </nav>
        </aside>

        <main class="main-content">
            <header class="main-header">
                <div class="milestones">
                    <svg
                        class="milestones-icon"
                        width="20"
                        height="20"
                        viewBox="0 0 20 20"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                    >
                        <path
                            d="M17.5 10.8333H15V17.5H5V10.8333H2.5L10 3.33333L17.5 10.8333Z"
                            stroke="currentColor"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path
                            d="M5 17.5V10.8333"
                            stroke="currentColor"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path
                            d="M15 17.5V10.8333"
                            stroke="currentColor"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                    </svg>
                    Milestones
                </div>
                <div class="header-right">
                    <div class="progress-bar">
                        <div class="progress-bar-fill" style="width: 6%"></div>
                        <span class="progress-text">6%</span>
                    </div>
                    <div class="search-container">
                        <svg
                            class="search-icon"
                            width="16"
                            height="16"
                            viewBox="0 0 16 16"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                        >
                            <circle
                                cx="6.66667"
                                cy="6.66667"
                                r="5.33333"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                            <path
                                d="M11.3333 11.3333L14.6667 14.6667"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                        </svg>
                        <input
                            type="text"
                            placeholder="Search"
                            class="search-input"
                        />
                        <span class="search-shortcuts">XK</span>
                    </div>
                    <div class="view-as">
                        View as
                        <select class="admin-select">
                            <option>Admin</option>
                        </select>
                    </div>
                    <button class="icon-button" aria-label="Notifications">
                        <svg
                            width="20"
                            height="20"
                            viewBox="0 0 20 20"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                        >
                            <path
                                d="M17.5 15.8333H15V17.5H5V15.8333H2.5L10 8.33333L17.5 15.8333Z"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                            <path
                                d="M5 17.5V15.8333"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                            <path
                                d="M15 17.5V15.8333"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                        </svg>
                    </button>
                    <button class="icon-button" aria-label="Messages">
                        <svg
                            width="20"
                            height="20"
                            viewBox="0 0 20 20"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                        >
                            <path
                                d="M17.5 15.8333H15V17.5H5V15.8333H2.5L10 8.33333L17.5 15.8333Z"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                            <path
                                d="M5 17.5V15.8333"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                            <path
                                d="M15 17.5V15.8333"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                        </svg>
                    </button>
                    <button class="icon-button" aria-label="User Profile">
                        <svg
                            width="20"
                            height="20"
                            viewBox="0 0 20 20"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                        >
                            <path
                                d="M17.5 15.8333H15V17.5H5V15.8333H2.5L10 8.33333L17.5 15.8333Z"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                            <path
                                d="M5 17.5V15.8333"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                            <path
                                d="M15 17.5V15.8333"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                        </svg>
                    </button>
                    <div class="user-profile">
                        <img
                            src="/path/to/user-avatar.png"
                            alt="User Avatar"
                            class="user-avatar"
                        />
                        <span class="user-name">Ilya</span>
                        <svg
                            class="dropdown-icon"
                            width="16"
                            height="16"
                            viewBox="0 0 16 16"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                        >
                            <path
                                d="M4 6L8 10L12 6"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                        </svg>
                    </div>
                    <button class="apply-button">Apply to Marketplace</button>
                </div>
            </header>

            <section class="content-section">
                <h1 class="section-title">Today</h1>
                <div class="today-stats-grid">
                    <div class="card daily-processing-card">
                        <div class="card-header">
                            <span>Daily processing</span>
                            <svg
                                class="info-icon"
                                width="16"
                                height="16"
                                viewBox="0 0 16 16"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <circle
                                    cx="8"
                                    cy="8"
                                    r="7.33333"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                />
                                <path
                                    d="M8 5.33337V8.00004"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M8 10.6667H8.00667"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                        </div>
                        <div class="card-body">
                            <div class="main-value">$8,492.11</div>
                            <div class="comparison-value">
                                <span class="increase-icon">+$1,490.00</span>
                                <svg
                                    width="12"
                                    height="12"
                                    viewBox="0 0 12 12"
                                    fill="none"
                                    xmlns="http://www.w3.org/2000/svg"
                                >
                                    <path
                                        d="M6 9L9 6L6 3"
                                        stroke="currentColor"
                                        stroke-width="1.5"
                                        stroke-linecap="round"
                                        stroke-linejoin="round"
                                    />
                                    <path
                                        d="M9 6H3"
                                        stroke="currentColor"
                                        stroke-width="1.5"
                                        stroke-linecap="round"
                                        stroke-linejoin="round"
                                    />
                                </svg>
                            </div>
                            <div class="secondary-value">$891.95</div>
                            <div class="chart-container">
                                <canvas id="dailyProcessingChart"></canvas>
                            </div>
                            <div class="chart-xaxis">
                                <span>10:00 AM</span>
                                <span>11:00 AM</span>
                                <span>12:00 PM</span>
                                <span>1:00 PM</span>
                                <span>2:00 PM</span>
                                <span>3:00 PM</span>
                                <span>4:00 PM</span>
                            </div>
                        </div>
                    </div>

                    <div class="card daily-stats-card">
                        <div class="card-header">
                            <span>Daily stats</span>
                            <svg
                                class="info-icon"
                                width="16"
                                height="16"
                                viewBox="0 0 16 16"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <circle
                                    cx="8"
                                    cy="8"
                                    r="7.33333"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                />
                                <path
                                    d="M8 5.33337V8.00004"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M8 10.6667H8.00667"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                        </div>
                        <div class="stats-grid">
                            <div class="stat-item">
                                <div class="stat-label">Revenue</div>
                                <div class="stat-values">
                                    <div class="stat-today">
                                        Today<span class="value">$891.95</span>
                                    </div>
                                    <div class="stat-yesterday">
                                        Yesterday<span class="value"
                                            >$645.95</span
                                        >
                                    </div>
                                </div>
                            </div>
                            <div class="stat-item">
                                <div class="stat-label">Customers</div>
                                <div class="stat-values">
                                    <div class="stat-today">
                                        Today<span class="value">36</span>
                                    </div>
                                    <div class="stat-yesterday">
                                        Yesterday<span class="value">32</span>
                                    </div>
                                </div>
                            </div>
                            <div class="stat-item">
                                <div class="stat-label">New signups</div>
                                <div class="stat-values">
                                    <div class="stat-today">
                                        Today<span class="value">12</span>
                                    </div>
                                    <div class="stat-yesterday">
                                        Yesterday<span class="value">8</span>
                                    </div>
                                </div>
                            </div>
                            <div class="stat-item">
                                <div class="stat-label">Resolution center</div>
                                <div class="stat-values">
                                    <div class="stat-today">
                                        Today<span class="value">3 cases</span>
                                    </div>
                                    <div class="stat-yesterday">
                                        Yesterday<span class="value"
                                            >1 case</span
                                        >
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="stats-section">
                    <div class="stats-tabs">
                        <button class="tab-button">Stats</button>
                        <div class="tab-dropdown">
                            <select class="past-weeks-select">
                                <option>Past 2 weeks</option>
                            </select>
                        </div>
                        <div class="date-range">Nov 20th - Dec 4th</div>
                        <div class="comparison">compared to</div>
                        <div class="tab-dropdown">
                            <select class="previous-period-select">
                                <option>Previous period</option>
                            </select>
                        </div>
                        <div class="tab-dropdown">
                            <select class="year-select">
                                <option>2023</option>
                            </select>
                        </div>
                        <button class="add-button">
                            <svg
                                width="16"
                                height="16"
                                viewBox="0 0 16 16"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    d="M8 13.3333V2.66663"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M13.3333 8H2.66663"
                                    stroke="currentColor"
                                    stroke-width="1.5"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                            Add
                        </button>
                        <button class="edit-button">Edit</button>
                    </div>

                    <div class="stats-charts-grid">
                        <div class="card gross-revenue-card">
                            <div class="card-header">
                                <span>Gross revenue</span>
                                <svg
                                    class="info-icon"
                                    width="16"
                                    height="16"
                                    viewBox="0 0 16 16"
                                    fill="none"
                                    xmlns="http://www.w3.org/2000/svg"
                                >
                                    <circle
                                        cx="8"
                                        cy="8"
                                        r="7.33333"
                                        stroke="currentColor"
                                        stroke-width="1.33333"
                                    />
                                    <path
                                        d="M8 5.33337V8.00004"
                                        stroke="currentColor"
                                        stroke-width="1.33333"
                                        stroke-linecap="round"
                                        stroke-linejoin="round"
                                    />
                                    <path
                                        d="M8 10.6667H8.00667"
                                        stroke="currentColor"
                                        stroke-width="1.33333"
                                        stroke-linecap="round"
                                        stroke-linejoin="round"
                                    />
                                </svg>
                            </div>
                            <div class="card-body">
                                <div class="main-value">$52,402.55</div>
                                <div class="comparison-value">
                                    <span class="increase-icon"
                                        >+$3,804.15</span
                                    >
                                    <svg
                                        width="12"
                                        height="12"
                                        viewBox="0 0 12 12"
                                        fill="none"
                                        xmlns="http://www.w3.org/2000/svg"
                                    >
                                        <path
                                            d="M6 9L9 6L6 3"
                                            stroke="currentColor"
                                            stroke-width="1.5"
                                            stroke-linecap="round"
                                            stroke-linejoin="round"
                                        />
                                        <path
                                            d="M9 6H3"
                                            stroke="currentColor"
                                            stroke-width="1.5"
                                            stroke-linecap="round"
                                            stroke-linejoin="round"
                                        />
                                    </svg>
                                </div>
                                <div class="secondary-value">$8,032.50</div>
                                <div class="chart-container">
                                    <canvas id="grossRevenueChart"></canvas>
                                </div>
                                <div class="chart-xaxis">
                                    <span>Nov 20th</span>
                                    <span>Dec 4th</span>
                                </div>
                            </div>
                        </div>

                        <div class="card gross-revenue-card">
                            <div class="card-header">
                                <span>Gross revenue</span>
                                <svg
                                    class="info-icon"
                                    width="16"
                                    height="16"
                                    viewBox="0 0 16 16"
                                    fill="none"
                                    xmlns="http://www.w3.org/2000/svg"
                                >
                                    <circle
                                        cx="8"
                                        cy="8"
                                        r="7.33333"
                                        stroke="currentColor"
                                        stroke-width="1.33333"
                                    />
                                    <path
                                        d="M8 5.33337V8.00004"
                                        stroke="currentColor"
                                        stroke-width="1.33333"
                                        stroke-linecap="round"
                                        stroke-linejoin="round"
                                    />
                                    <path
                                        d="M8 10.6667H8.00667"
                                        stroke="currentColor"
                                        stroke-width="1.33333"
                                        stroke-linecap="round"
                                        stroke-linejoin="round"
                                    />
                                </svg>
                            </div>
                            <div class="card-body">
                                <div class="main-value">$3,304.35</div>
                                <div class="comparison-value">
                                    <span class="increase-icon">+$634.10</span>
                                    <svg
                                        width="12"
                                        height="12"
                                        viewBox="0 0 12 12"
                                        fill="none"
                                        xmlns="http://www.w3.org/2000/svg"
                                    >
                                        <path
                                            d="M6 9L9 6L6 3"
                                            stroke="currentColor"
                                            stroke-width="1.5"
                                            stroke-linecap="round"
                                            stroke-linejoin="round"
                                        />
                                        <path
                                            d="M9 6H3"
                                            stroke="currentColor"
                                            stroke-width="1.5"
                                            stroke-linecap="round"
                                            stroke-linejoin="round"
                                        />
                                    </svg>
                                </div>
                                <div class="secondary-value">$2,900.15</div>
                                <div class="chart-container">
                                    <canvas id="grossRevenueChart2"></canvas>
                                </div>
                                <div class="chart-xaxis">
                                    <span>Nov 20th</span>
                                    <span>Dec 4th</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="lower-section-grid">
                    <div class="card net-revenue-card">
                        <div class="card-header">
                            <span>Net revenue</span>
                            <svg
                                class="info-icon"
                                width="16"
                                height="16"
                                viewBox="0 0 16 16"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <circle
                                    cx="8"
                                    cy="8"
                                    r="7.33333"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                />
                                <path
                                    d="M8 5.33337V8.00004"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M8 10.6667H8.00667"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                        </div>
                        <div class="card-body">
                            <div class="main-value">$32,030.55</div>
                            <div class="comparison-value">
                                <span class="decrease-icon">-$690.55</span>
                                <svg
                                    width="12"
                                    height="12"
                                    viewBox="0 0 12 12"
                                    fill="none"
                                    xmlns="http://www.w3.org/2000/svg"
                                >
                                    <path
                                        d="M6 3L3 6L6 9"
                                        stroke="currentColor"
                                        stroke-width="1.5"
                                        stroke-linecap="round"
                                        stroke-linejoin="round"
                                    />
                                    <path
                                        d="M3 6H9"
                                        stroke="currentColor"
                                        stroke-width="1.5"
                                        stroke-linecap="round"
                                        stroke-linejoin="round"
                                    />
                                </svg>
                            </div>
                            <div class="secondary-value">12.7K</div>
                            <div class="chart-container">
                                <canvas id="netRevenueChart"></canvas>
                            </div>
                            <div class="chart-xaxis">
                                <span>Nov 20th</span>
                                <span>Dec 4th</span>
                            </div>
                        </div>
                    </div>

                    <div class="card top-customers-card">
                        <div class="card-header">
                            <span>Top customers</span>
                            <svg
                                class="info-icon"
                                width="16"
                                height="16"
                                viewBox="0 0 16 16"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <circle
                                    cx="8"
                                    cy="8"
                                    r="7.33333"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                />
                                <path
                                    d="M8 5.33337V8.00004"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M8 10.6667H8.00667"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                        </div>
                        <div class="card-body">
                            <div class="customer-list">
                                <div class="customer-item">
                                    <img
                                        src="/path/to/avatar-stephanie.png"
                                        alt="Stephanie Avatar"
                                        class="customer-avatar"
                                    />
                                    <span class="customer-name"
                                        >Stephanie#4877</span
                                    >
                                    <span class="total-spend">$34,510.50</span>
                                    <span class="status-badge active"
                                        >14 days</span
                                    >
                                </div>
                                <div class="customer-item">
                                    <img
                                        src="/path/to/avatar-julia.png"
                                        alt="Julia Avatar"
                                        class="customer-avatar"
                                    />
                                    <span class="customer-name"
                                        >Julia#2401</span
                                    >
                                    <span class="total-spend">$32,020.00</span>
                                    <span class="status-badge active"
                                        >14 days</span
                                    >
                                </div>
                                <div class="customer-item">
                                    <img
                                        src="/path/to/avatar-emma.png"
                                        alt="Emma Avatar"
                                        class="customer-avatar"
                                    />
                                    <span class="customer-name">Emma#4409</span>
                                    <span class="total-spend">$25,400.00</span>
                                    <span class="status-badge active"
                                        >14 days</span
                                    >
                                </div>
                                <div class="customer-item">
                                    <img
                                        src="/path/to/avatar-jon.png"
                                        alt="Jon Avatar"
                                        class="customer-avatar"
                                    />
                                    <span class="customer-name">Jon#0093</span>
                                    <span class="total-spend">$19,835.45</span>
                                    <span class="status-badge warning"
                                        >3 days</span
                                    >
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card payments-card">
                        <div class="card-header">
                            <span>Payments</span>
                            <svg
                                class="info-icon"
                                width="16"
                                height="16"
                                viewBox="0 0 16 16"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <circle
                                    cx="8"
                                    cy="8"
                                    r="7.33333"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                />
                                <path
                                    d="M8 5.33337V8.00004"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M8 10.6667H8.00667"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                        </div>
                        <div class="card-body">
                            <div class="payment-progress-bar">
                                <div
                                    class="bar-segment successful"
                                    style="width: 22%"
                                ></div>
                                <div
                                    class="bar-segment past"
                                    style="width: 13%"
                                ></div>
                                <div
                                    class="bar-segment due"
                                    style="width: 37%"
                                ></div>
                                <div
                                    class="bar-segment failed"
                                    style="width: 22%"
                                ></div>
                                <div
                                    class="bar-segment refunded"
                                    style="width: 6%"
                                ></div>
                            </div>
                            <div class="payment-stats-list">
                                <div class="payment-stat-item">
                                    <span
                                        class="status-color successful"
                                    ></span>
                                    <span class="status-label">Successful</span>
                                    <span class="count">481</span>
                                    <span class="percentage">(22%)</span>
                                </div>
                                <div class="payment-stat-item">
                                    <span class="status-color past"></span>
                                    <span class="status-label">Past</span>
                                    <span class="count">202</span>
                                    <span class="percentage">(13%)</span>
                                </div>
                                <div class="payment-stat-item">
                                    <span class="status-color due"></span>
                                    <span class="status-label">Due</span>
                                    <span class="count">534</span>
                                    <span class="percentage">(37%)</span>
                                </div>
                                <div class="payment-stat-item">
                                    <span class="status-color failed"></span>
                                    <span class="status-label">Failed</span>
                                    <span class="count">495</span>
                                    <span class="percentage">(22%)</span>
                                </div>
                                <div class="payment-stat-item">
                                    <span class="status-color refunded"></span>
                                    <span class="status-label">Refunded</span>
                                    <span class="count">128</span>
                                    <span class="percentage">(6%)</span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card marketplace-card">
                        <div class="card-header">
                            <span>Marketplace</span>
                            <svg
                                class="info-icon"
                                width="16"
                                height="16"
                                viewBox="0 0 16 16"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <circle
                                    cx="8"
                                    cy="8"
                                    r="7.33333"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                />
                                <path
                                    d="M8 5.33337V8.00004"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                                <path
                                    d="M8 10.6667H8.00667"
                                    stroke="currentColor"
                                    stroke-width="1.33333"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                />
                            </svg>
                        </div>
                        <div class="card-body">
                            <div class="marketplace-tabs">
                                <button class="marketplace-tab active">
                                    Sales
                                </button>
                                <button class="marketplace-tab">GMV</button>
                                <button class="marketplace-tab">MRR</button>
                            </div>
                            <div class="marketplace-list">
                                <div class="marketplace-item">
                                    <span class="rank">1.</span>
                                    <span class="name">James#2230</span>
                                    <span class="value">+$65.30</span>
                                </div>
                                <div class="marketplace-item">
                                    <span class="rank">2.</span>
                                    <span class="name">Jon#0093</span>
                                    <span class="value">+$58.55</span>
                                </div>
                                <div class="marketplace-item">
                                    <span class="rank">3.</span>
                                    <span class="name">Tyler#0394</span>
                                    <span class="value">+$51.00</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </main>
    </div>
</template>

<script setup>
import { ref } from "vue";

// Placeholder for chart initialization if needed
// const dailyProcessingChart = ref(null);
// const grossRevenueChart = ref(null);
// const grossRevenueChart2 = ref(null);
// const netRevenueChart = ref(null);

// Example of emitting an event if a button was clicked
const emit = defineEmits(["applyMarketplace"]);

const handleApplyMarketplace = () => {
    emit("applyMarketplace");
};
</script>

<style scoped>
:root {
    --color-background: #1a1a1a;
    --color-sidebar-bg: #202020;
    --color-card-bg: #252525;
    --color-primary-text: #ffffff;
    --color-secondary-text: #b3b3b3;
    --color-accent: #007bff;
    --color-border: #333333;
    --color-chart-line: #6a0dad;
    --color-chart-area: rgba(106, 13, 173, 0.2);
    --color-increase: #10b981;
    --color-decrease: #ef4444;
    --color-warning: #f59e0b;
    --color-success: #10b981;
    --color-due: #6366f1;
    --color-failed: #ef4444;
    --color-refunded: #9ca3af;
    --color-progress-bar-bg: #333333;
    --color-progress-bar-fill: #007bff;
    --color-input-bg: #333333;
    --color-select-bg: #333333;
    --color-icon-default: #ffffff;
    --color-icon-secondary: #b3b3b3;
}

.dashboard-container {
    display: flex;
    min-height: 100vh;
    background-color: var(--color-background);
    color: var(--color-primary-text);
    font-family: "Inter", sans-serif;
}

.sidebar {
    width: 280px;
    background-color: var(--color-sidebar-bg);
    padding: 20px;
    display: flex;
    flex-direction: column;
    border-right: 1px solid var(--color-border);
}

.sidebar-header {
    display: flex;
    align-items: center;
    margin-bottom: 30px;
    padding-bottom: 15px;
    border-bottom: 1px solid var(--color-border);
}

.logo {
    width: 30px;
    height: 30px;
    margin-right: 10px;
}

.brand-name {
    font-size: 18px;
    font-weight: 600;
}

.settings-button {
    margin-left: auto;
    background: none;
    border: none;
    color: var(--color-secondary-text);
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 5px;
    border-radius: 4px;
    transition: background-color 0.2s ease;
}

.settings-button:hover {
    background-color: var(--color-card-bg);
}

.sidebar-nav ul {
    list-style: none;
    padding: 0;
    margin: 0;
}

.nav-item {
    margin-bottom: 10px;
}

.nav-link {
    display: flex;
    align-items: center;
    padding: 10px 15px;
    border-radius: 6px;
    text-decoration: none;
    color: var(--color-secondary-text);
    font-size: 15px;
    font-weight: 500;
    transition:
        background-color 0.2s ease,
        color 0.2s ease;
    position: relative;
}

.nav-link .icon {
    margin-right: 12px;
    color: var(--color-icon-secondary);
}

.nav-item.active .nav-link {
    background-color: var(--color-card-bg);
    color: var(--color-primary-text);
}

.nav-item.active .nav-link .icon {
    color: var(--color-primary-text);
}

.nav-item .badge {
    background-color: var(--color-warning);
    color: var(--color-primary-text);
    font-size: 11px;
    font-weight: 600;
    padding: 2px 6px;
    border-radius: 4px;
    margin-left: auto;
}

.nav-link:hover {
    background-color: var(--color-card-bg);
    color: var(--color-primary-text);
}

.nav-link:hover .icon {
    color: var(--color-primary-text);
}

.has-submenu > .nav-link::after {
    content: "";
    display: block;
    width: 16px;
    height: 16px;
    margin-left: auto;
    background-image: url("data:image/svg+xml,%3Csvg width='16' height='16' viewBox='0 0 16 16' fill='none' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath d='M4 6L8 10L12 6' stroke='currentColor' stroke-width='1.5' stroke-linecap='round' stroke-linejoin='round'/%3E%3C/svg%3E");
    background-repeat: no-repeat;
    background-position: center;
    color: var(--color-secondary-text);
    transition: transform 0.2s ease;
}

.has-submenu.open > .nav-link::after {
    transform: rotate(180deg);
}

.submenu {
    list-style: none;
    padding: 0;
    margin: 5px 0 0 0;
    padding-left: 20px;
    display: none;
}

.has-submenu.open .submenu {
    display: block;
}

.submenu li a {
    display: block;
    padding: 8px 15px;
    text-decoration: none;
    color: var(--color-secondary-text);
    font-size: 14px;
    font-weight: 500;
    border-radius: 4px;
    transition:
        background-color 0.2s ease,
        color 0.2s ease;
}

.submenu li a:hover {
    background-color: #303030;
    color: var(--color-primary-text);
}

.main-content {
    flex-grow: 1;
    padding: 20px;
    display: flex;
    flex-direction: column;
}

.main-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 30px;
    padding-bottom: 15px;
    border-bottom: 1px solid var(--color-border);
}

.milestones {
    display: flex;
    align-items: center;
    font-size: 18px;
    font-weight: 600;
}

.milestones-icon {
    margin-right: 10px;
    color: var(--color-icon-default);
}

.header-right {
    display: flex;
    align-items: center;
}

.progress-bar {
    position: relative;
    width: 80px;
    height: 20px;
    background-color: var(--color-progress-bar-bg);
    border-radius: 10px;
    margin-right: 20px;
    overflow: hidden;
}

.progress-bar-fill {
    position: absolute;
    top: 0;
    left: 0;
    height: 100%;
    background-color: var(--color-progress-bar-fill);
    border-radius: 10px;
    transition: width 0.3s ease;
}

.progress-text {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    font-size: 12px;
    font-weight: 600;
    color: var(--color-primary-text);
}

.search-container {
    display: flex;
    align-items: center;
    background-color: var(--color-input-bg);
    padding: 8px 12px;
    border-radius: 6px;
    margin-right: 20px;
}

.search-icon {
    color: var(--color-secondary-text);
    margin-right: 8px;
}

.search-input {
    background: none;
    border: none;
    color: var(--color-primary-text);
    font-size: 14px;
    outline: none;
    flex-grow: 1;
}

.search-input::placeholder {
    color: var(--color-secondary-text);
}

.search-shortcuts {
    font-size: 12px;
    font-weight: 600;
    color: var(--color-secondary-text);
    background-color: rgba(255, 255, 255, 0.1);
    padding: 2px 5px;
    border-radius: 3px;
}

.view-as {
    display: flex;
    align-items: center;
    font-size: 14px;
    color: var(--color-secondary-text);
    margin-right: 20px;
}

.admin-select {
    background-color: var(--color-select-bg);
    color: var(--color-primary-text);
    border: none;
    padding: 5px 10px;
    border-radius: 4px;
    margin-left: 8px;
    font-size: 14px;
    cursor: pointer;
    appearance: none;
    background-image: url("data:image/svg+xml,%3Csvg width='16' height='16' viewBox='0 0 16 16' fill='none' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath d='M4 6L8 10L12 6' stroke='currentColor' stroke-width='1.5' stroke-linecap='round' stroke-linejoin='round'/%3E%3C/svg%3E");
    background-repeat: no-repeat;
    background-position: right 8px center;
    padding-right: 25px;
}

.icon-button {
    background: none;
    border: none;
    color: var(--color-secondary-text);
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 5px;
    border-radius: 4px;
    margin-left: 5px;
    transition: background-color 0.2s ease;
}

.icon-button:hover {
    background-color: var(--color-card-bg);
}

.user-profile {
    display: flex;
    align-items: center;
    margin-left: 15px;
    cursor: pointer;
}

.user-avatar {
    width: 30px;
    height: 30px;
    border-radius: 50%;
    margin-right: 8px;
}

.user-name {
    font-weight: 500;
}

.dropdown-icon {
    color: var(--color-secondary-text);
    margin-left: 5px;
}

.apply-button {
    background-color: var(--color-accent);
    color: var(--color-primary-text);
    border: none;
    padding: 10px 15px;
    border-radius: 6px;
    font-size: 14px;
    font-weight: 600;
    cursor: pointer;
    margin-left: 20px;
    transition: background-color 0.2s ease;
}

.apply-button:hover {
    background-color: #0056b3;
}

.content-section {
    display: flex;
    flex-direction: column;
    flex-grow: 1;
}

.section-title {
    font-size: 24px;
    font-weight: 700;
    margin-bottom: 25px;
}

.today-stats-grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 20px;
    margin-bottom: 30px;
}

.card {
    background-color: var(--color-card-bg);
    border-radius: 10px;
    padding: 20px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 15px;
    font-size: 16px;
    font-weight: 600;
}

.info-icon {
    color: var(--color-secondary-text);
    cursor: pointer;
}

.card-body {
    display: flex;
    flex-direction: column;
}

.daily-processing-card .card-body {
    align-items: center;
}

.main-value {
    font-size: 32px;
    font-weight: 700;
    margin-bottom: 5px;
}

.comparison-value {
    display: flex;
    align-items: center;
    font-size: 14px;
    font-weight: 600;
    margin-bottom: 15px;
}

.comparison-value .increase-icon {
    color: var(--color-increase);
    margin-right: 5px;
}

.comparison-value .decrease-icon {
    color: var(--color-decrease);
    margin-right: 5px;
}

.secondary-value {
    font-size: 14px;
    color: var(--color-secondary-text);
    margin-bottom: 20px;
}

.chart-container {
    width: 100%;
    height: 150px;
    position: relative;
}

.chart-xaxis {
    display: flex;
    justify-content: space-between;
    font-size: 12px;
    color: var(--color-secondary-text);
    margin-top: 10px;
}

.daily-stats-card .stats-grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 15px;
}

.stat-item {
    display: flex;
    flex-direction: column;
}

.stat-label {
    font-size: 14px;
    color: var(--color-secondary-text);
    margin-bottom: 8px;
}

.stat-values {
    display: flex;
    flex-direction: column;
}

.stat-today,
.stat-yesterday {
    font-size: 16px;
    font-weight: 500;
    display: flex;
    flex-direction: column;
}

.stat-today .value,
.stat-yesterday .value {
    font-size: 18px;
    font-weight: 600;
    color: var(--color-primary-text);
}

.stat-yesterday {
    margin-top: 5px;
    color: var(--color-secondary-text);
}

.stats-section {
    margin-bottom: 30px;
}

.stats-tabs {
    display: flex;
    align-items: center;
    margin-bottom: 20px;
}

.tab-button {
    background: none;
    border: none;
    color: var(--color-secondary-text);
    font-size: 16px;
    font-weight: 600;
    margin-right: 15px;
    cursor: pointer;
    padding: 5px 0;
    position: relative;
}

.tab-button.active {
    color: var(--color-primary-text);
}

.tab-button.active::after {
    content: "";
    position: absolute;
    bottom: -5px;
    left: 0;
    width: 100%;
    height: 2px;
    background-color: var(--color-accent);
    border-radius: 1px;
}

.tab-dropdown {
    margin-right: 15px;
}

.past-weeks-select,
.previous-period-select,
.year-select {
    background-color: var(--color-select-bg);
    color: var(--color-primary-text);
    border: none;
    padding: 8px 12px;
    border-radius: 6px;
    font-size: 14px;
    cursor: pointer;
    appearance: none;
    background-image: url("data:image/svg+xml,%3Csvg width='16' height='16' viewBox='0 0 16 16' fill='none' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath d='M4 6L8 10L12 6' stroke='currentColor' stroke-width='1.5' stroke-linecap='round' stroke-linejoin='round'/%3E%3C/svg%3E");
    background-repeat: no-repeat;
    background-position: right 8px center;
    padding-right: 25px;
}

.comparison {
    font-size: 14px;
    color: var(--color-secondary-text);
    margin-right: 15px;
}

.add-button,
.edit-button {
    background: none;
    border: none;
    color: var(--color-secondary-text);
    font-size: 14px;
    font-weight: 500;
    cursor: pointer;
    padding: 8px 12px;
    border-radius: 6px;
    transition:
        background-color 0.2s ease,
        color 0.2s ease;
}

.add-button {
    display: flex;
    align-items: center;
    margin-left: auto;
}

.add-button .icon {
    margin-right: 8px;
}

.add-button:hover,
.edit-button:hover {
    background-color: var(--color-card-bg);
    color: var(--color-primary-text);
}

.stats-charts-grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 20px;
}

.gross-revenue-card .card-body {
    align-items: center;
}

.lower-section-grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 20px;
}

.net-revenue-card .card-body {
    align-items: center;
}

.top-customers-card .card-body {
    display: flex;
    flex-direction: column;
}

.customer-list {
    display: flex;
    flex-direction: column;
    gap: 15px;
}

.customer-item {
    display: flex;
    align-items: center;
}

.customer-avatar {
    width: 30px;
    height: 30px;
    border-radius: 50%;
    margin-right: 10px;
}

.customer-name {
    font-weight: 500;
    flex-grow: 1;
}

.total-spend {
    font-size: 14px;
    color: var(--color-secondary-text);
    margin-right: 15px;
}

.status-badge {
    padding: 4px 8px;
    border-radius: 4px;
    font-size: 12px;
    font-weight: 600;
}

.status-badge.active {
    background-color: rgba(16, 185, 121, 0.1);
    color: var(--color-success);
}

.status-badge.warning {
    background-color: rgba(245, 158, 11, 0.1);
    color: var(--color-warning);
}

.payments-card .card-body {
    display: flex;
    flex-direction: column;
}

.payment-progress-bar {
    display: flex;
    height: 10px;
    border-radius: 5px;
    overflow: hidden;
    margin-bottom: 20px;
}

.bar-segment {
    height: 100%;
}

.bar-segment.successful {
    background-color: var(--color-success);
}

.bar-segment.past {
    background-color: var(--color-due);
}

.bar-segment.due {
    background-color: var(--color-due);
}

.bar-segment.failed {
    background-color: var(--color-failed);
}

.bar-segment.refunded {
    background-color: var(--color-refunded);
}

.payment-stats-list {
    display: flex;
    flex-direction: column;
    gap: 12px;
}

.payment-stat-item {
    display: flex;
    align-items: center;
}

.status-color {
    width: 12px;
    height: 12px;
    border-radius: 3px;
    margin-right: 8px;
}

.status-color.successful {
    background-color: var(--color-success);
}

.status-color.past {
    background-color: var(--color-due);
}

.status-color.due {
    background-color: var(--color-due);
}

.status-color.failed {
    background-color: var(--color-failed);
}

.status-color.refunded {
    background-color: var(--color-refunded);
}

.status-label {
    font-size: 14px;
    font-weight: 500;
    flex-grow: 1;
}

.count {
    font-weight: 500;
    margin-right: 5px;
}

.percentage {
    font-size: 13px;
    color: var(--color-secondary-text);
}

.marketplace-card .card-body {
    display: flex;
    flex-direction: column;
}

.marketplace-tabs {
    display: flex;
    margin-bottom: 15px;
    border-bottom: 1px solid var(--color-border);
}

.marketplace-tab {
    background: none;
    border: none;
    color: var(--color-secondary-text);
    font-size: 14px;
    font-weight: 600;
    padding: 8px 15px;
    cursor: pointer;
    position: relative;
    transition: color 0.2s ease;
}

.marketplace-tab.active {
    color: var(--color-primary-text);
}

.marketplace-tab.active::after {
    content: "";
    position: absolute;
    bottom: -1px;
    left: 0;
    width: 100%;
    height: 2px;
    background-color: var(--color-accent);
}

.marketplace-list {
    display: flex;
    flex-direction: column;
    gap: 15px;
}

.marketplace-item {
    display: flex;
    align-items: center;
}

.rank {
    font-size: 14px;
    color: var(--color-secondary-text);
    margin-right: 10px;
    width: 20px;
}

.name {
    font-weight: 500;
    flex-grow: 1;
}

.value {
    font-weight: 600;
    color: var(--color-increase);
}

@media (max-width: 1200px) {
    .today-stats-grid,
    .stats-charts-grid,
    .lower-section-grid {
        grid-template-columns: 1fr;
    }
}

@media (max-width: 768px) {
    .sidebar {
        width: 60px;
        padding: 10px;
    }
    .sidebar-header .brand-name {
        display: none;
    }
    .sidebar-nav ul li a span:not(.badge) {
        display: none;
    }
    .sidebar-nav ul li a .icon {
        margin-right: 0;
    }
    .main-header {
        flex-wrap: wrap;
        justify-content: center;
    }
    .header-right {
        width: 100%;
        justify-content: space-between;
        margin-top: 15px;
    }
    .search-container {
        flex-grow: 1;
        margin-right: 10px;
    }
    .apply-button {
        margin-left: 0;
        white-space: nowrap;
    }
    .section-title {
        font-size: 20px;
    }
    .today-stats-grid,
    .stats-charts-grid,
    .lower-section-grid {
        grid-template-columns: 1fr;
    }
    .card {
        padding: 15px;
    }
    .main-value {
        font-size: 28px;
    }
    .comparison-value {
        font-size: 13px;
    }
    .stat-today .value,
    .stat-yesterday .value {
        font-size: 16px;
    }
    .customer-name {
        font-size: 14px;
    }
    .total-spend {
        font-size: 13px;
        margin-right: 10px;
    }
    .status-badge {
        font-size: 11px;
    }
}

@media (max-width: 480px) {
    .main-header {
        flex-direction: column;
        align-items: flex-start;
    }
    .header-right {
        width: 100%;
        flex-wrap: wrap;
        justify-content: flex-start;
        margin-top: 15px;
    }
    .search-container {
        width: calc(100% - 100px);
        margin-right: 10px;
    }
    .apply-button {
        width: 100%;
        margin-top: 10px;
    }
    .stats-tabs {
        flex-wrap: wrap;
    }
    .tab-button,
    .tab-dropdown,
    .comparison {
        margin-bottom: 10px;
    }
    .add-button {
        margin-left: 0;
        margin-top: 10px;
    }
}
</style>
