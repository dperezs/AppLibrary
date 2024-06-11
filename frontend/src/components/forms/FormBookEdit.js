import { useEffect, useState } from "react";
import { endpoints } from '../../api/endpoints';
import { useLocation } from 'react-router-dom';
import { useNavigate  } from 'react-router-dom';

import {
  Card,
  Row,
  Col,
  CardTitle,
  CardBody,
  Button,
  Form,
  FormGroup,
  Label,
  Input,
  Alert,
  } from "reactstrap";
  
  const FormBookEdit = (prop) => {
    const location = useLocation();
    const { book } = location.state;
    const [formBook, setformBook] = useState({});
    const [visible, setVisible] = useState(false);
    const [mensajeError, setMensajeError] = useState("");
    const navigate = useNavigate();
    useEffect(()=> {
        console.log(book);
        const tempBook = {
          ...book,          
        }
        setformBook(tempBook);
    },[])
    const handleChangeForm = (event) => {
      if (event.target)
        {
          if (event.target.id === "Name") {
            const tempform = { ...formBook, name: event.target.value };
            setformBook(tempform);
          }
          if (event.target.id === "Copies") {
            const tempform = { ...formBook, noCopies: event.target.value };
            setformBook(tempform);
          }
          if (event.target.id === "Description") {
            const tempform = { ...formBook, description: event.target.value };
            setformBook(tempform);
          }
        }
    }
    const handleGuardar = () => {
      if (formBook.id>0){
        endpoints.FormBook.Update(formBook)
        .then(response => {							
          if (response.message){
            setMensajeError(response.message);          
          }						
          else{          
            navigate("/formBook");
          }													          
        });      
      }else {
        endpoints.FormBook.Add(formBook)
            .then(response => {   
              if (response.mensajeError)                     
                setMensajeError(response.message);          
              else
                navigate("/formBook");
            });
            
      }
      
    }

    const onDismiss = () => {
      setVisible(false);
    };
    return (
      <><Row>
      <Col>
        {/* --------------------------------------------------------------------------------*/}
        {/* Card-1*/}
        {/* --------------------------------------------------------------------------------*/}
        <Card>
          <CardTitle tag="h6" className="border-bottom p-3 mb-0">
            <i className="bi bi-bell me-2"> </i>
            Form Example
          </CardTitle>
          <CardBody>
            <Form>
              <FormGroup>
                <Label for="exampleEmail">Nombre</Label>
                <Input id="Name"
                 defaultValue={formBook.name} onChange={(e) => handleChangeForm(e)}
                />
              </FormGroup>
              <FormGroup>
                <Label for="exampleEmail">No. Copias</Label>
                <Input id="Copies" type="number"
                 defaultValue={formBook.noCopies} onChange={(e) => handleChangeForm(e)}
                />                    
              </FormGroup>
              <FormGroup>
                <Label for="exampleEmail">Descripción</Label>
                <Input id="Description"
                 defaultValue={formBook.description} onChange={(e) => handleChangeForm(e)}
                />                    
              </FormGroup>                  
              <Button onClick={() => handleGuardar()}>Submit</Button>
            </Form>
          </CardBody>
        </Card>
      </Col>
    </Row>
    <Alert
        color="primary"
        isOpen={visible}
        toggle={onDismiss.bind(null)}
        fade={true}
        >
        I am a primary alert and I can be dismissed without animating!
    </Alert>
      </>
        
        
      );
  }
  export default FormBookEdit;