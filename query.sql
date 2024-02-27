select * from category;
select p.*, c.Name from product p
inner join Category c on c.Id = p.CategoryId;
select * from supplier;

select count(*) from Supply;
select count(*) from supplyproduct;

-- Database First Approach
-- Code First Approach
-- Database Code First Approach
