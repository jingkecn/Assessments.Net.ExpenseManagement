Assessment: Design an Expense Management System
===============================================

- [Scenario](#scenario)

Scenario
--------

Fake, Inc. (a fictional company) is launching a new [expense management](https://en.wikipedia.org/wiki/Expense_management) service to handle a wide range of employee-initiated expenses. Authorized employees can submit expense claims, which are then reviewed and processed by relevant departments within the company.

This domain is inherently complex and spans multiple business concerns, including:

- Submitting and managing expense claims
- Auditing and approving expenses
- Tracking the processing lifecycle
- Managing employee profiles and entitlements
- Storing and analyzing historical financial data for compliance and insights

In addition to the domain complexity, the company is growing rapidly and prioritizes agility in delivering new features and scaling the system. The application is expected to:

- Operate at cloud scale
- Meet high service level objectives (SLOs)
- Support diverse and evolving data storage and querying needs across different functional areas

Given these requirements, a microservices architecture is a natural fit. It allows the system to be decomposed into independently deployable, domain-aligned services that can scale and evolve at different rates. This architecture also supports:

- Clear separation of concerns aligned with business capabilities
- Flexible technology choices per service (e.g., optimized storage engines or processing models)
- Improved resilience and fault isolation
- Faster iteration cycles for teams working on different parts of the system

As a result, Fake, Inc. has chosen a microservices approach to build a scalable, maintainable, and adaptable Expense Management platform.
