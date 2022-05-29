/*
Author: Mark Doghramji
LinkedIn: https://www.linkedin.com/in/mark-doghramji/
Last Update: 5/29/2022
Notes:
This class acts as an abstract representation of an XML reader. Since most of the game will have large quantities of text data being
imported by .xml files it makes sense to have some abstraction involved. All child classes of XML reader will have certain methods,
that will do similiar actions but not identically  (i.e. load text data from and XML and populate a Object Array with Object"s")
*/

using System.Collections;
using System.Collections.Generic;
using System.Xml;

public abstract class XMLReader
{
    /*Each XmlReader child class inheriting from this class will need to calculate the number of core XMLnodes.
    This is because each Reader class will be returning an array of Object(s) created from the particular reader.*/
    protected abstract int calculateNumberOfCoreXMLNodes();


    //protected abstract XMLReader load
    
}
