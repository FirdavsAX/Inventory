select * from saleProduct where saleid in (select id from sale where customerId = 1);
