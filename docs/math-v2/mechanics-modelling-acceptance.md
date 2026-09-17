# Mechanics / Modelling V2 shadow acceptance

## Scope

This slice adds bounded, exact-rational mechanics models with mandatory canonical SI dimensional validation. It is a shadow capability slice only; it does not change learner-facing routing.

Supported model operations:

- constant acceleration: `v = u + at`;
- constant acceleration: `s = ut + 1/2 at²`;
- Newton's second law: `F = ma`;
- Newton's third law: equal and opposite one-dimensional reaction force;
- one-dimensional equilibrium: balancing force that makes the resultant zero;
- impulse-momentum: final momentum = initial momentum + impulse;
- work-energy: final kinetic energy = initial kinetic energy + net work;
- average power: work / elapsed time.

## Quantity representation and dimensional analysis

Every model input and output is represented as:

`quantity_si(exactValue, canonicalUnit)`

Canonical units admitted by this slice are:

- mass: `kg`;
- length/displacement: `m`;
- time: `s`;
- velocity: `m_per_s`;
- acceleration: `m_per_s2`;
- force: `N`;
- momentum: `kg_m_per_s`;
- impulse: `N_s`;
- work/energy: `J`;
- power: `W`.

The solver rejects a quantity whose unit has the wrong physical dimension for the declared model. Unit aliases/conversions outside this canonical SI subset are intentionally unsupported in this slice.

## Physical fail-closed bounds

- elapsed time in constant-acceleration models cannot be negative;
- mass in Newton's second law must be positive;
- equilibrium force vectors contain `1..8` forces;
- initial kinetic energy cannot be negative;
- work-energy requests that produce negative kinetic energy are rejected;
- average-power elapsed time must be positive;
- malformed rational values and unsupported quantity shapes are rejected;
- exact-rational numerators and denominators are bounded to 4096 bits with bounded intermediate arithmetic;
- no floating-point fallback is used.

## Independent verification

`ExactMechanicsModelVerifier` independently reconstructs each supported relationship from the original request. It has separate unit/dimension lookup and exact-arithmetic code from the solver and does not trust the solver's result or trace.

Mutation tests require changed numeric outputs, changed output units, dimension-invalid requests and physically invalid forged solved results to fail verification.

## Generation

Eight deterministic solver-grounded shadow question families cover the supported operations. Every generated item:

1. declares exact SI quantities;
2. is solved from the declared physical model;
3. passes independent verification before it can be returned by the factory;
4. records model/unit metadata;
5. remains `ShadowVerified` with `productionRouting:false`.

## Routing

This phase does **not** map Mechanics V2 directly to curriculum outcomes and does **not** cut Practice, Assessment or Diagnostic traffic over to the new families. Curriculum gates and product migration occur in later roadmap phases after strategy, equivalence, adaptive intelligence and curriculum-specific capability gates are proven.
