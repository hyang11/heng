package Main;

import java.awt.*;
import java.awt.event.*;
import java.sql.*;

import javax.swing.*;

public class login implements ActionListener {
	JButton btnD = new JButton(" 登陆 ");
	JButton btnZ = new JButton(" 注册 ");
	JFrame loginform = new JFrame();
	JTextField username = new JTextField();
	JPasswordField password = new JPasswordField();
	Connection con;

	@Override
	public void actionPerformed(ActionEvent e) {
		// TODO 自动生成的方法存根
		if (e.getSource() == btnD) {
			if(password.getPassword().equals(logins(username.getText()))){
				userform u = new userform();
			}
		} else if (e.getSource() == btnZ) {
			reg r = new reg();
		}
	}

	login() {
		Panel panel1 = new Panel();
		Panel panel2 = new Panel();
		Panel panel3 = new Panel();
		Panel panel4 = new Panel();
		panel1.setLayout(new FlowLayout());
		panel2.setLayout(new FlowLayout());
		panel3.setLayout(new FlowLayout());
		panel4.setLayout(new FlowLayout());
		JLabel labeltitel = new JLabel();
		username = new JTextField(15);
		password = new JPasswordField(15);
		labeltitel.setText("欢迎使用同学通讯录");
		labeltitel.setFont(new Font("", 3, 30));
		panel1.add(labeltitel);
		panel2.add(new JLabel("用户名"));
		panel2.add(username);
		panel3.add(new JLabel("   密码 "));
		panel3.add(password);
		panel4.add(btnD);
		panel4.add(btnZ);
		loginform.setSize(320, 300);
		loginform.setVisible(true);
		loginform.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		loginform.setLayout(new GridLayout(4, 1));
		loginform.setLocation(500, 250);
		loginform.add(panel1);
		loginform.add(panel2);
		loginform.add(panel3);
		loginform.add(panel4);
		loginform.validate();
		btnD.addActionListener(this);
		btnZ.addActionListener(this);
	}

	public static void main(String[] args) {
		new login();
	}
	
	public String logins(String name) {

		String sql = "select * from 用户  where 用户名 =" + "'" + name + "'";
		try {
			con = DriverManager
					.getConnection(
							"jdbc:odbc:Driver={MicroSoft Access Driver (*.mdb)};DBQ=D:/同学通讯录.mdb",
							"dba", "sql");
			Statement s = con.createStatement();
			ResultSet rs = s.executeQuery(sql);
			while (rs.next()) {
				return rs.getString("密码");
			}
			s.close();
			con.close();
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		return null;
	}


}
