Overview
========

\Coherence\ partial classes are responsible for making sure that the DSL diagram cannot be put into
a state that does not match the domain rules.

Specifically:-
--------------

1) No projection may handle an event for a different aggregate identifier than the one to which the 
projection is linked.  This means that a projection only runs over one event stream.

2) No aggregate identifier may have multiple parent aggregate identifiers.  This is to allow the creation
of consistent hierarchies for the REST API for the domain.

