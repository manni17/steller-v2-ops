# Technical Product Manager - First Week Plan

**Status: SUPERSEDED** – Do not use for AI agent workflows.

**Replaced by:** [TPM_STABILIZATION_AUDIT_PROTOCOL.md](./TPM_STABILIZATION_AUDIT_PROTOCOL.md)

**Reason:** This plan assumes human-style onboarding (5 days of discovery, meetings). AI agents can parse the system in minutes. Use the 1-Hour Forensic Audit Protocol instead.

---

## Executive Summary (Historical – For Reference Only)

As a newly hired Technical Product Manager for Steller, my first week will focus on understanding the current system state, production health, business context, and establishing a foundation for effective product management. This plan prioritizes **non-invasive assessment** and **knowledge gathering** before making any changes.

**Key Principle:** Observe, understand, document - then act.

---

## Day 1: System Discovery & Production Health Assessment

### Morning: Infrastructure Audit

**Objective:** Understand what's running, where, and its health status.

**Tasks:**

1. **Container & Service Inventory**
   - ✅ Verify running containers (Docker ps completed)
   - Document all services and their ports
   - Check resource usage (CPU, memory, disk)
   - Verify network connectivity between services

2. **Production Health Check**
   - Test API endpoints (`GET /api/health`)
   - Check database connectivity and performance
   - Review recent logs for errors/warnings
   - Verify background job processing (Hangfire)

3. **System Architecture Review**
   - Read canonical architecture docs (`docs/architecture/systems.yaml`, `containers.yaml`)
   - Map out system boundaries and integrations
   - Understand protected systems (steller-v2, steller-legacy)
   - Document external dependencies (Bamboo vendor API)

**Deliverables:**
- Production health dashboard snapshot
- Infrastructure inventory document
- System architecture diagram (current state)

### Afternoon: Codebase Exploration

**Objective:** Understand code structure, tech stack, and development patterns.

**Tasks:**

1. **Technology Stack Assessment**
   - Identify primary languages/frameworks (.NET 9, PostgreSQL, Redis)
   - Review project structure and organization
   - Understand build/deployment processes
   - Check for CI/CD pipelines

2. **Code Quality Review**
   - Review test coverage and testing patterns
   - Check for documentation standards
   - Understand code review processes
   - Identify technical debt areas

3. **Architecture Patterns**
   - Review ADRs (Architecture Decision Records)
   - Understand API design patterns
   - Review authentication/authorization model
   - Check data flow patterns

**Deliverables:**
- Tech stack summary
- Codebase health assessment
- Architecture patterns documentation

---

## Day 2: Business Context & Product Understanding

### Morning: Product Discovery

**Objective:** Understand what Steller does, who uses it, and how it generates value.

**Tasks:**

1. **Product Overview**
   - Read product documentation and growth strategy plan
   - Understand B2B gift card platform model
   - Identify key user personas (Partners, Admins)
   - Map user journeys (signup → integration → orders)

2. **Business Model Analysis**
   - Understand revenue model (margin-based)
   - Review pricing structure
   - Identify key business metrics
   - Understand partner lifecycle

3. **Market & Competitive Context**
   - Review any competitive analysis
   - Understand market positioning
   - Identify differentiation factors

**Deliverables:**
- Product overview document
- User persona map
- Business model summary

### Afternoon: Current Features & Roadmap

**Objective:** Understand what exists today and what's planned.

**Tasks:**

1. **Feature Inventory**
   - List all API endpoints (from `apis.yaml`)
   - Document current capabilities
   - Identify feature gaps vs. growth strategy plan
   - Review partner onboarding flow

2. **Roadmap Review**
   - Review growth strategy plan priorities
   - Understand Phase 1-4 implementation plan
   - Identify dependencies and blockers
   - Review success metrics and goals

3. **Stakeholder Mapping**
   - Identify key stakeholders (internal/external)
   - Understand partner needs and pain points
   - Document admin requirements

**Deliverables:**
- Feature inventory
- Roadmap analysis
- Stakeholder map

---

## Day 3: Production Monitoring & Metrics

### Morning: Monitoring & Observability

**Objective:** Understand how we monitor production and what metrics we track.

**Tasks:**

1. **Monitoring Infrastructure**
   - Identify monitoring tools (if any)
   - Check for application logs
   - Review error tracking
   - Check for performance monitoring

2. **Metrics Analysis**
   - Review current metrics (if available)
   - Identify missing critical metrics
   - Understand analytics infrastructure status
   - Check API usage patterns

3. **Alerting & Incident Response**
   - Review alerting setup
   - Understand on-call processes
   - Check incident history
   - Review runbooks/documentation

**Deliverables:**
- Monitoring assessment
- Metrics gap analysis
- Incident response process documentation

### Afternoon: Database & Data Analysis

**Objective:** Understand data model and current usage patterns.

**Tasks:**

1. **Database Schema Review**
   - Review database schema (Partners, Orders, Products, etc.)
   - Understand data relationships
   - Check for data quality issues
   - Review migration history

2. **Usage Pattern Analysis**
   - Query partner activity (if analytics exist)
   - Review order volumes and patterns
   - Identify high-value partners
   - Check for inactive partners

3. **Data Integrity Check**
   - Verify referential integrity
   - Check for orphaned records
   - Review data retention policies
   - Assess backup/recovery processes

**Deliverables:**
- Database schema documentation
- Usage pattern analysis
- Data quality assessment

---

## Day 4: Risk Assessment & Technical Debt

### Morning: Production Risks

**Objective:** Identify immediate risks to production stability and business continuity.

**Tasks:**

1. **Infrastructure Risks**
   - Single point of failure analysis
   - Resource capacity planning
   - Backup/disaster recovery assessment
   - Security posture review

2. **Operational Risks**
   - Vendor dependency risks (Bamboo API)
   - API rate limiting and throttling
   - Error handling and circuit breakers
   - Background job reliability

3. **Business Risks**
   - Partner onboarding friction
   - Missing growth infrastructure (analytics, self-service)
   - Legacy system dependencies
   - Scalability concerns

**Deliverables:**
- Risk register
- Priority risk mitigation plan
- Business continuity assessment

### Afternoon: Technical Debt & Quality

**Objective:** Understand technical debt and quality issues.

**Tasks:**

1. **Code Quality Assessment**
   - Review test coverage
   - Identify code smells
   - Check for security vulnerabilities
   - Review dependency versions

2. **Architecture Debt**
   - Legacy system dependencies
   - Missing documentation
   - Inconsistent patterns
   - Performance bottlenecks

3. **Process Debt**
   - Development workflow gaps
   - Missing CI/CD automation
   - Documentation gaps
   - Onboarding friction

**Deliverables:**
- Technical debt inventory
- Quality metrics baseline
- Improvement recommendations

---

## Day 5: Strategy & Planning

### Morning: Gap Analysis

**Objective:** Compare current state vs. growth strategy plan.

**Tasks:**

1. **Growth Strategy Alignment**
   - Review Phase 1 priorities (self-service signup, API logging, analytics)
   - Identify implementation blockers
   - Assess readiness for each phase
   - Review gap analysis from plan

2. **Critical Gaps Assessment**
   - Email service existence (GAP-001)
   - Database schema verification (GAP-002, GAP-003)
   - Redis queue operations (GAP-004)
   - Wallet API verification (GAP-005)

3. **Dependency Mapping**
   - Map task dependencies
   - Identify critical path
   - Assess resource requirements
   - Review timeline feasibility

**Deliverables:**
- Gap analysis report
- Implementation readiness assessment
- Dependency map

### Afternoon: First Week Summary & Recommendations

**Objective:** Synthesize learnings and provide actionable recommendations.

**Tasks:**

1. **Key Findings Summary**
   - Production health status
   - Critical risks identified
   - Technical debt priorities
   - Growth strategy readiness

2. **Immediate Action Items**
   - Top 3 priorities for next week
   - Quick wins identified
   - Critical blockers to resolve
   - Resource needs

3. **30-Day Plan Outline**
   - Month 1 objectives
   - Key milestones
   - Success metrics
   - Stakeholder communication plan

**Deliverables:**
- First week summary report
- Immediate action plan
- 30-day roadmap outline

---

## Key Questions to Answer This Week

### Technical
- [ ] What is the current API uptime and error rate?
- [ ] How many active partners do we have?
- [ ] What is the current order volume?
- [ ] Are there any production incidents or outages?
- [ ] What is the database size and growth rate?
- [ ] Are backups working correctly?

### Business
- [ ] What is the current revenue run rate?
- [ ] What is the partner activation rate?
- [ ] What are the main partner pain points?
- [ ] What is the churn rate?
- [ ] What is the average order value?
- [ ] What is the partner lifetime value?

### Product
- [ ] What features are most used?
- [ ] What features are missing?
- [ ] What is blocking partner growth?
- [ ] What is the developer experience like?
- [ ] How easy is partner onboarding?
- [ ] What is the API documentation quality?

### Process
- [ ] How are features prioritized?
- [ ] What is the development workflow?
- [ ] How are bugs tracked and resolved?
- [ ] What is the release process?
- [ ] How is customer feedback collected?
- [ ] What is the team structure?

---

## Tools & Resources Needed

### Access Required
- [ ] Production database read access
- [ ] API endpoint access (test API keys)
- [ ] Monitoring/logging access
- [ ] Code repository access
- [ ] Documentation access
- [ ] Stakeholder contact list

### Documentation to Review
- [x] `docs/architecture/systems.yaml` - System boundaries
- [x] `docs/architecture/containers.yaml` - Container/port mapping
- [x] `docs/architecture/atlas/apis.yaml` - API contracts
- [x] `.cursor/plans/steller_product_lead_growth_strategy_e28a2ede.plan.md` - Growth strategy
- [ ] `docs/STELLER_QA_AGENT_PROTOCOL_V2.md` - QA processes
- [ ] ADRs in `docs/architecture/decisions/`
- [ ] Any runbooks or operational docs

### Stakeholders to Meet
- [ ] Owner (you)
- [ ] Development team (if exists)
- [ ] Key partners (if accessible)
- [ ] Any other stakeholders

---

## Success Criteria for First Week

### Must Have
- ✅ Complete production health assessment
- ✅ Understand system architecture
- ✅ Identify top 3 risks
- ✅ Document current state baseline
- ✅ Create immediate action plan

### Nice to Have
- [ ] Meet key stakeholders
- [ ] Review partner feedback
- [ ] Complete gap analysis
- [ ] Create 30-day roadmap
- [ ] Identify quick wins

---

## Notes & Observations

### Current System State (Initial Assessment)

**Running Services:**
- ✅ Steller v2 API (port 6091) - Healthy, up 12 minutes
- ✅ Steller v2 PostgreSQL (port 6432) - Healthy, up 13 hours
- ✅ Steller v2 Redis (port 6379) - Healthy, up 13 hours
- ✅ Legacy Admin Dashboard (port 8080) - Up 5 days
- ✅ Legacy Consumer Dashboard (port 8081) - Up 5 days
- ✅ Legacy PostgreSQL (port 5432) - Up 5 days
- ✅ Legacy RabbitMQ (port 5672) - Up 5 days
- ✅ OpenClaw Gateway (port 3001) - Up 8 hours

**Key Observations:**
- Steller v2 API was recently restarted (12 minutes ago)
- Legacy systems are stable (5 days uptime)
- Multiple systems running on same VPS
- Protected systems identified (steller-v2, steller-legacy)

**Immediate Questions:**
1. Why was Steller v2 API restarted recently?
2. What is the relationship between Steller v2 and legacy systems?
3. What is OpenClaw Gateway and how does it relate to Steller?
4. Are there any monitoring/alerting systems in place?

---

## Next Steps After First Week

1. **Week 2 Focus:** Begin addressing critical gaps (GAP-001 through GAP-005)
2. **Week 2 Focus:** Start Phase 1 implementation planning (self-service signup)
3. **Week 2 Focus:** Set up basic monitoring/metrics if missing
4. **Week 2 Focus:** Establish regular stakeholder communication rhythm

---

## Appendix: Quick Reference

### System Ports
- **6091:** Steller v2 API
- **6432:** Steller v2 PostgreSQL
- **6379:** Steller v2 Redis
- **8080:** Legacy Admin Dashboard
- **8081:** Legacy Consumer Dashboard
- **5432:** Legacy PostgreSQL
- **5672:** Legacy RabbitMQ
- **3001:** OpenClaw Gateway

### Key Directories
- `/opt/steller-v2/` - Steller v2 codebase
- `/opt/steller/` - Legacy Steller codebase
- `/root/docs/architecture/` - Architecture documentation
- `/root/.cursor/plans/` - Product plans

### Key Commands
```bash
# Check container status
docker ps --filter name=steller-v2

# Check API health
curl http://localhost:6091/api/health

# View API logs
docker logs steller-v2-api

# Connect to database
docker exec -it steller-v2-postgres psql -U steller_v2_user -d steller_v2
```

---

**Document Owner:** Technical Product Manager  
**Last Updated:** February 18, 2026  
**Status:** Active Plan
