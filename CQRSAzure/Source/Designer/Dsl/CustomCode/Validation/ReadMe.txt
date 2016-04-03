Validation
==========

Partial classes in this folder perform the validations specific to each of the model components to
prevent the model being saved in a state that would either cause failure to generate or generate code
that was not correct.

These classes rely on the \Interfaces to enforce the requirements that other parts of the code can place 
on these domain classes.