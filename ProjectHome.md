# Project Tracker Objectfactory #


ProjectTrackerObjectFactory is a project I am putting out to let other people use as an example for the new [Csla](http://www.lhotka.net) Object Factory methods. It is designed to be a drop in replacement for the standard [Csla](http://www.lhotka.net) ProjectTracker.Library.dll.


## Dependencies: ##
  * [Csla 3.6](http://www.lhotka.net)
  * [NHibernate](http://sourceforge.net/projects/nhibernate)
  * [Fluent NHibernate](http://code.google.com/p/fluent-nhibernate/)
  * Log4net
  * [StructureMap 2.5](http://structuremap.sourceforge.net/)
  * [RhinoMocks](http://ayende.com/projects/rhino-mocks.aspx)
  * Other dependencies these rely on.


It uses NHibernate as the ORM on all DAL tasks. There is no DTO objects in this design.


I am not finished implementing all the tests on this project but intend to keep the source code updated as I complete them. And I would also like to say this is just one way you can do this, any contributions / critiques are welcome.

Note: 10/21/2008 - I just updated the zip file to include the latest changes in SVN. Most of these changes are tests and code cleanup but we did a lot of it. As always, the latest is always available in the trunk.