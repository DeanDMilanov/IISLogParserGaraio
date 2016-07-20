# IISLogParserGaraio
Coding interview project
Delivered functionality:
-read log files (.txt, .log), and analyze them - using regex, search for unique IP addresses, keep track of the count, and get FQDN info from a domain server when possible.
- uses multithreading to speed up queries.
- sacrifices throughput to reduce latency of a single request.
I've used some functional programming idioms, such as Option<T>, which shows that a method may return null, without having to look at its implementation.
