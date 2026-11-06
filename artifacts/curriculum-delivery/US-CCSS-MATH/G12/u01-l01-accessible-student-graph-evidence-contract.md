# Section 1.1 graph-response contract

Original Precalculus 2017 graphical task families:
- Exercises **40–59**: classify/source-read given coordinate graphs, including
  vertical/horizontal line reasoning and graph values. An optional labelled
  coordinate-description field is exposed.
- Exercises **76–87**: explicitly construct the function graph within a specified
  source x-window and report its output range. A textual representation of
  labelled axes, vertex/intersections and plotted coordinates is mandatory;
  teacher reviews its adequacy together with the authored source-window graph.

The first-party student form combines a written explanation and labelled graph
coordinates in a single tenant-scoped SQL response. Validation is done on the
server: valid 1–105 activity ID; explanation length 20–8000; optional graph
evidence max 3000; evidence required for 76–87 and at least 20 characters;
combined stored payload max 8000. HTML is Razor-escaped in the teacher view.
Teacher rubric remains four binary criteria, max four marks.

This is a legitimate **accessible typed alternative**, not a drawn graph upload,
not a verified auto-grader, not source figure pixel-parity, and not a substitute
for the teacher checking the graphical construction. Stage D is intentionally
not executed in A–B–C.
