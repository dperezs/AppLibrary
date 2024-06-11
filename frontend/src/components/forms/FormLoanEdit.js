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
  
  const FormLoanEdit = (prop) => {
    const location = useLocation();
    const { book } = location.state;
    const [formLoan, setformLoand] = useState({});
    const [listaBooks, setListaBooks] = useState([]);
    const [visible, setVisible] = useState(false);
    const [mensajeError, setMensajeError] = useState("");
    const navigate = useNavigate();
    useEffect(()=> {
        console.log(book);
        Promise.all([
            endpoints.FormBook.GetAll()
        ])
            .then(res => {
                if (res.some(r => r.message)) {
                    setVisible(true);
                    setMensajeError('Ocurrio un error al cargar los datos');
                    return;
                }
                const [ents] = res;
                const books = ents.data
                .map((i) => ({
                    value: i.id,
                    label: i.name             
                 }));

                    setListaBooks(books);
            });
        
        const tempBook = {
          ...book,          
        }
        setformLoand(tempBook);
    },[])
    const handleChangeForm = (event) => {
      if (event.target)
        {
          if (event.target.id === "usuario") {
            const tempform = { ...formLoan, person: event.target.value };
            setformLoand(tempform);
          }
          if (event.target.id === "libros") {
            const tempform = { ...formLoan, bookId: event.target.value };
            setformLoand(tempform);
          }
          if (event.target.id === "status") {
            const tempform = { ...formLoan, status: event.target.value };
            setformLoand(tempform);
          }
        }
    }
    const handleGuardar = () => {
      if (formLoan.id>0){
        endpoints.FormLoans.Update(formLoan)
        .then(response => {							
          if (response.message){
            setVisible(true);
            setMensajeError(response.message);          
          }						
          else{          
            navigate("/formLoans");
          }													          
        });      
      }else {
        endpoints.FormLoans.Add(formLoan)
            .then(response => {   
              if (response.message) {
                setVisible(true);
                setMensajeError(response.message);          
              }               
                
              else
                navigate("/formLoans");
            });
            
      }
      
    }

    const onDismiss = () => {
      setVisible(false);
    };
    return (
      <>
       <Alert
        color="primary"
        isOpen={visible}
        toggle={onDismiss.bind(null)}
        fade={true}
        >
        {mensajeError}
    </Alert>
      <Row>
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
                <Label for="exampleEmail">Usuario</Label>
                <Input id="usuario"
                 defaultValue={formLoan.person} onChange={(e) => handleChangeForm(e)}
                />
              </FormGroup>
              <FormGroup>
                <Label for="exampleSelectMulti">Libro</Label>
                <Input
                  id="libros"                  
                  name="selectMulti"
                  type="select"
                  value={formLoan.bookId} onChange={e => handleChangeForm(e)}
                >
                 <option value=""> -- Seleccione un libro -- </option>
                 {listaBooks.map((obj) => <option key={obj.value} value={obj.value}>{obj.label}</option>)}
                </Input>
              </FormGroup>
              <FormGroup>
                <Label for="exampleSelectMulti">Estatus</Label>
                <Input
                  id="status"
                  name="selectMulti"
                  type="select"
                  value={formLoan.status} onChange={e => handleChangeForm(e)}
                >
                  <option> -- Seleccione un estatus -- </option>
                  <option >Prestamo</option>
                  <option>Devolución</option>                  
                </Input>
              </FormGroup>
              <Button onClick={() => handleGuardar()}>Submit</Button>
            </Form>
          </CardBody>
        </Card>
      </Col>
    </Row>
   
      </>
        
        
      );
  }
  export default FormLoanEdit;